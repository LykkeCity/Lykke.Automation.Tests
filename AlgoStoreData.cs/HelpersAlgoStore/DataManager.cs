using AlgoStoreData.DTOs;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon;
using XUnitTestCommon.Utils;

namespace AlgoStoreData.Fixtures
{
    public static class DataManager
    {
        private static List<BuilInitialDataObjectDTO> PreStoredMetadata = new List<BuilInitialDataObjectDTO>();
        private static Random rnd = new Random();
        private static List<int> metaDataIndexIDWithFile = new List<int>();

        public static void storeMetadata(List<BuilInitialDataObjectDTO> preStoredMetadata)
        {
            PreStoredMetadata = preStoredMetadata;
        }

        public static BuilInitialDataObjectDTO getMetadataForEdit()
        {
            lock (PreStoredMetadata)
            {
                int r = rnd.Next(PreStoredMetadata.Count);
                BuilInitialDataObjectDTO editMetadataResponceDTO = PreStoredMetadata[r];
                return editMetadataResponceDTO;
            }
        }

        public static BuilInitialDataObjectDTO getMetaDataForStringUpload()
        {
            lock (PreStoredMetadata)
            {
                int r;
                do
                {
                    r = rnd.Next(PreStoredMetadata.Count);
                }
                while (metaDataIndexIDWithFile.Contains(r));
                BuilInitialDataObjectDTO editMetadataResponceDTO = PreStoredMetadata[r];
                metaDataIndexIDWithFile.Add(r);
                return editMetadataResponceDTO;
            }
        }

        public static BuilInitialDataObjectDTO getMetadataForDelete()
        {
            lock (PreStoredMetadata)
            {
                int r = rnd.Next(PreStoredMetadata.Count);
                BuilInitialDataObjectDTO editMetadataResponceDTO = PreStoredMetadata[r];
                PreStoredMetadata.RemoveAt(r);
                return editMetadataResponceDTO;
            }
        }

        public static List<BuilInitialDataObjectDTO> getAllMetaData()
        {
            lock (PreStoredMetadata)
            {
                return PreStoredMetadata;
            }
        }
    }
}
