using System;

namespace EndlessLegenedSaveEditor
{
    class EmpireStats
    {
        public EmpireStats(string empireName, Uri empirePortraitPath, string bankAccount, 
            string empirePointStock, string empireResearchStock, string strategic1Stock, 
            string strategic2Stock, string strategic3Stock, string strategic4Stock, 
            string strategic5Stock, string strategic6Stock)
        {
            EmpireName = empireName;
            EmpirePortraitPath = empirePortraitPath;
            BankAccount = bankAccount;
            EmpirePointStock = empirePointStock;
            EmpireResearchStock = empireResearchStock;
            Strategic1Stock = strategic1Stock;
            Strategic2Stock = strategic2Stock;
            Strategic3Stock = strategic3Stock;
            Strategic4Stock = strategic4Stock;
            Strategic5Stock = strategic5Stock;
            Strategic6Stock = strategic6Stock;
        }
        public string EmpireName { get; set; }
        public Uri EmpirePortraitPath { get; set; }
        public string BankAccount { get; set; }
        public string EmpirePointStock { get; set; }
        public string EmpireResearchStock { get; set; }
        public string Strategic1Stock { get; set; }
        public string Strategic2Stock { get; set; }
        public string Strategic3Stock { get; set; }
        public string Strategic4Stock { get; set; }
        public string Strategic5Stock { get; set; }
        public string Strategic6Stock { get; set; }
    }
}
