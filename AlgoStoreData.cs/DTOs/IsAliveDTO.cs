using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    public class IsAliveDTO
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Env { get; set; }
        public bool IsDebug {get; set;}
        public ArrayList IssueIndicators { get; set; }
    }
}
