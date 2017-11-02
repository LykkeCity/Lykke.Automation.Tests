using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AssetsData.DTOs.Assets
{
    public class AssetSettingsDTO
    {
        public string Id
        {
            get { return Asset; }
            private set { }
        }
        public string Asset { get; set; }
        public string CashinCoef { get; set; }
        public string ChangeWallet { get; set; }
        public string Dust { get; set; }
        public string HotWallet { get; set; }
        public string MaxBalance { get; set; }
        public int MaxOutputsCount { get; set; }
        public int MaxOutputsCountInTx { get; set; }
        public string MinBalance { get; set; }
        public int MinOutputsCount { get; set; }
        public string OutputSize { get; set; }
        public int PrivateIncrement { get; set; }

        public void NormalizeNumberStrings(AssetSettingsDTO parsedDTO)
        {
            NormalizeNumberString("MinBalance", parsedDTO); 
            NormalizeNumberString("MaxBalance", parsedDTO);
            NormalizeNumberString("OutputSize", parsedDTO);
            NormalizeNumberString("CashinCoef", parsedDTO);
            NormalizeNumberString("Dust", parsedDTO);
        }

        private void NormalizeNumberString(string propertyName, AssetSettingsDTO parsedDTO)
        {
            PropertyInfo property = this.GetType().GetProperty(propertyName);
            if (property != null)
            {
                object localValueObject = property.GetValue(this);
                object parsedValueObject = property.GetValue(parsedDTO);
                if (localValueObject != null && parsedValueObject != null)
                {
                    string localValue = localValueObject.ToString();
                    string parsedValue = parsedValueObject.ToString();
                    if (localValue.Length > parsedValue.Length)
                    {
                        property.SetValue(this, localValue.Substring(0, parsedValue.Length));
                    }
                }
            }
        }
    }

    public class AllAssetSettingsDTO
    {
        public List<AssetSettingsDTO> Items { get; set; }
    }
}
