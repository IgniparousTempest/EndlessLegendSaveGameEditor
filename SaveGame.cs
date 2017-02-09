using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;

namespace EndlessLegenedSaveEditor
{
    class SaveGame
    {
        private const string gameXmlPath = @"Endless Legend/Game.xml";

        private const string xmlRelativePathBankAccount = "./SimulationObject/Properties/Property[@Name='BankAccount']";
        private const string xmlRelativePathEmpirePointStock = "./SimulationObject/Properties/Property[@Name='EmpirePointStock']";
        private const string xmlRelativePathEmpireResearchStock = "./SimulationObject/Properties/Property[@Name='EmpireResearchStock']";
        private const string xmlRelativePathStrategic1Stock = "./SimulationObject/Properties/Property[@Name='Strategic1Stock']";
        private const string xmlRelativePathStrategic2Stock = "./SimulationObject/Properties/Property[@Name='Strategic2Stock']";
        private const string xmlRelativePathStrategic3Stock = "./SimulationObject/Properties/Property[@Name='Strategic3Stock']";
        private const string xmlRelativePathStrategic4Stock = "./SimulationObject/Properties/Property[@Name='Strategic4Stock']";
        private const string xmlRelativePathStrategic5Stock = "./SimulationObject/Properties/Property[@Name='Strategic5Stock']";
        private const string xmlRelativePathStrategic6Stock = "./SimulationObject/Properties/Property[@Name='Strategic6Stock']";

        private readonly string saveGamePath;
        private XmlDocument xmlDoc;
        private List<EmpireStats> majorEmpires;

        public SaveGame (string saveGamePath)
        {
            this.saveGamePath = saveGamePath;
            xmlDoc = LoadGameSaveXml(this.saveGamePath);
        }

        public List<EmpireStats> MajorEmpires
        {
            get
            {
                if (majorEmpires == null)
                {
                    majorEmpires = new List<EmpireStats>();
                    foreach(var empireXml in GetEmpiresXML(xmlDoc))
                    {
                        majorEmpires.Add(CreateEmpireStats(empireXml));
                    }
                }

                return majorEmpires;
            }
        }

        private List<XmlNode> GetEmpiresXML(XmlDocument xmlRoot)
        {
            var empires = xmlRoot.DocumentElement.SelectSingleNode("descendant::Game/Empires");
            List<XmlNode> majorEmpires = new List<XmlNode>();
            foreach (XmlNode empire in empires.ChildNodes)
            {
                if (empire.Name == "MajorEmpire")
                    majorEmpires.Add(empire);
            }
            return majorEmpires;
        }

        public void UpdateSaveFile()
        {
            foreach (var empire in MajorEmpires.Zip(GetEmpiresXML(xmlDoc), (x, y) => new Tuple<EmpireStats, XmlNode>(x, y)))
                UpdateXmlWithEmpireStats(empire.Item2, empire.Item1);

            ZipArchiveEntry entry;
            using (ZipArchive zip = ZipFile.Open(saveGamePath, ZipArchiveMode.Update))
            {
                entry = zip.GetEntry(gameXmlPath);
                entry.Delete();
                zip.CreateEntry(gameXmlPath);
                entry = zip.GetEntry(gameXmlPath);
                using (StreamWriter writer = new StreamWriter(entry.Open()))
                {
                    writer.Write(xmlDoc.OuterXml);
                }
            }
        }

        XmlDocument LoadGameSaveXml(string zipfile)
        {
            Console.WriteLine("User selected: " + zipfile);
            ZipArchiveEntry entry;
            StreamReader sr;
            XmlDocument doc;
            using (ZipArchive zip = ZipFile.Open(zipfile, ZipArchiveMode.Read))
            {
                entry = zip.GetEntry(gameXmlPath);
                foreach (var item in zip.Entries)
                {
                    Console.WriteLine(item.FullName);
                }
                if (entry != null)
                {
                    sr = new StreamReader(entry.Open());
                    doc = new XmlDocument();
                    doc.LoadXml(sr.ReadToEnd());
                }
                else
                    throw new ArgumentException();
            }

            return doc;
        }

        private string TranslateEmpireName(XmlNode empireXml)
        {
            switch (empireXml.Attributes["FactionDescriptor"].Value)
            {
                default:
                    return empireXml.Attributes["FactionDescriptor"].Value.Split(',')[1];
            }
        }

        private Uri GetEmpirePortrait(XmlNode empireXml)
        {
            string imageUri;

            switch (TranslateEmpireName(empireXml))
            {
                case ("Necrophages"):
                    imageUri = @"/Images/Faction_necrophages.png";
                    break;
                case ("Drakken"):
                    imageUri = @"/Images/Faction_drakken.png";
                    break;
                case ("Broken Lords"):
                    imageUri = @"/Images/Faction_broken_lords.png";
                    break;
                case ("Mezari"):
                    imageUri = @"/Images/Faction_vaulters.png";
                    break;
                case ("Vaulters"):
                    imageUri = @"/Images/Faction_vaulters.png";
                    break;
                case ("Allayi"):
                    imageUri = @"/Images/Faction_Allayi.png";
                    break;
                case ("Wild Walkers"):
                    imageUri = @"/Images/Faction_wild_walkers.png";
                    break;
                case ("Ardent Mages"):
                    imageUri = @"/Images/Faction_Ardent_Mages.png";
                    break;
                default:
                    imageUri = @"/Images/Faction_wild_walkers.png";
                    break;
            }

            return new Uri(imageUri, UriKind.Relative);
        }

        protected EmpireStats CreateEmpireStats(XmlNode empireXml)
        {
            return new EmpireStats(
                TranslateEmpireName(empireXml),
                GetEmpirePortrait(empireXml),
                empireXml.SelectSingleNode(xmlRelativePathBankAccount).InnerText,
                empireXml.SelectSingleNode(xmlRelativePathEmpirePointStock).InnerText,
                empireXml.SelectSingleNode(xmlRelativePathEmpireResearchStock).InnerText,
                empireXml.SelectSingleNode(xmlRelativePathStrategic1Stock).InnerText,
                empireXml.SelectSingleNode(xmlRelativePathStrategic2Stock).InnerText,
                empireXml.SelectSingleNode(xmlRelativePathStrategic3Stock).InnerText,
                empireXml.SelectSingleNode(xmlRelativePathStrategic4Stock).InnerText,
                empireXml.SelectSingleNode(xmlRelativePathStrategic5Stock).InnerText,
                empireXml.SelectSingleNode(xmlRelativePathStrategic6Stock).InnerText
                );
        }

        private void UpdateXmlWithEmpireStats(XmlNode empireXml, EmpireStats empireStats)
        {
            empireXml.SelectSingleNode(xmlRelativePathBankAccount).InnerText = empireStats.BankAccount;
            empireXml.SelectSingleNode(xmlRelativePathEmpirePointStock).InnerText = empireStats.EmpirePointStock;
            empireXml.SelectSingleNode(xmlRelativePathEmpireResearchStock).InnerText = empireStats.EmpireResearchStock;
            empireXml.SelectSingleNode(xmlRelativePathStrategic1Stock).InnerText = empireStats.Strategic1Stock;
            empireXml.SelectSingleNode(xmlRelativePathStrategic2Stock).InnerText = empireStats.Strategic2Stock;
            empireXml.SelectSingleNode(xmlRelativePathStrategic3Stock).InnerText = empireStats.Strategic3Stock;
            empireXml.SelectSingleNode(xmlRelativePathStrategic4Stock).InnerText = empireStats.Strategic4Stock;
            empireXml.SelectSingleNode(xmlRelativePathStrategic5Stock).InnerText = empireStats.Strategic5Stock;
            empireXml.SelectSingleNode(xmlRelativePathStrategic6Stock).InnerText = empireStats.Strategic6Stock;
        }
    }
}
