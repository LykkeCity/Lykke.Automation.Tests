using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Entities.Assets
{
    public class WatchListEntity : TableEntity, IWatchList
    {
        public string Id => RowKey;
        public string Name { get; set; }
        public int Order { get; set; }
        public bool ReadOnly { get; set; }
        public string AssetIds
        {
            get
            {
                return this._assetIdsString;
            }
            set
            {
                this._assetIdsString = value;
                if (string.IsNullOrEmpty(_assetIdsString))
                    this._assetIDsList = null;
                else
                    this._assetIDsList = value.Split(",").ToList();
            }
        }
        public List<string> AssetIDsList
        {
            get
            {
                return this._assetIDsList;
            }
            set
            {
                this._assetIDsList = value;
                if (value == null)
                    this._assetIdsString = null;
                else
                    this._assetIdsString = String.Join(",", value);

            }
        }

        private string _assetIdsString;
        private List<string> _assetIDsList;

    }
}
