using AlgoStoreData.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.Fixtures
{
    public static class DataManager
    {
        private static List<MetaDataResponseDTO> PreStoredMetadata = new List<MetaDataResponseDTO>();
        private static Random rnd = new Random();
        private static List<int> metaDataIndexIDWithFile = new List<int>();

        public static void addSingleMetadata(MetaDataResponseDTO metaData)
        {
            lock (PreStoredMetadata)
            {
                PreStoredMetadata.Add(metaData);
            }
        }

        public static void storeMetadata(List<MetaDataResponseDTO> preStoredMetadata)
        {
            PreStoredMetadata = preStoredMetadata;
        }

        public static MetaDataResponseDTO getMetadataForEdit()
        {
            lock (PreStoredMetadata)
            {
                int r = rnd.Next(PreStoredMetadata.Count);
                MetaDataResponseDTO editMetadataResponceDTO = PreStoredMetadata[r];
                return editMetadataResponceDTO;
            }
        }

        public static MetaDataResponseDTO getMetaDataForBinaryUpload()
        {
            lock (PreStoredMetadata)
            {
                int r;
                do
                {
                    r = rnd.Next(PreStoredMetadata.Count);
                }
                while (metaDataIndexIDWithFile.Contains(r));
                MetaDataResponseDTO editMetadataResponceDTO = PreStoredMetadata[r];
                metaDataIndexIDWithFile.Add(r);
                return editMetadataResponceDTO;
            }
        }

        public static MetaDataResponseDTO getMetadataForDelete()
        {
            lock (PreStoredMetadata)
            {
                int r = rnd.Next(PreStoredMetadata.Count);
                MetaDataResponseDTO editMetadataResponceDTO = PreStoredMetadata[r];
                PreStoredMetadata.RemoveAt(r);
                return editMetadataResponceDTO;
            }
        }

        public static List<MetaDataResponseDTO> getAllMetaData()
        {
            lock (PreStoredMetadata)
            {
                return PreStoredMetadata;
            }
        }
    }
}
