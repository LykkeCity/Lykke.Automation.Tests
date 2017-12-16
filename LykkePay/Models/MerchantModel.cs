using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LykkePay.Models
{
    public class MerchantModel : AbstractMerchant
    {
        private static string merchantId = "bitteller.test.1";
        private static string apiKey = "BILETTERTESTKEY";
        private static string privateKey = "-----BEGIN RSA PRIVATE KEY-----\n" +
                      "MIIEowIBAAKCAQEAq5hd5yDO3Eep9z//LuCo3gjJNxWVmFyn//OPe9OtmkqzMX6q\n" +
                      "r2KQLXBiMRMid3WV3bCON//RJxdLnh+nkzOx8rqtjgiWJn9Fmky/Ij2fXbAT7yKL\n" +
                      "3OSG7DT3yzB1nwZe5sBWsUwQTm34+ymcv9Iwunx9t7ClNLGgHNvV8aj/W6PxlWSB\n" +
                      "jA2QliVBup4DVTgTVW0Bvm7EyBtdscOFsvNFxj0Q/TyGIMCdCRgiF2FL5fO3X1iq\n" +
                      "/NdnFT3hkQlEH4JLux3SkCQzHKPCys+8e6snGoeooM4oXeaTDLqBE+m1Myuym+Js\n" +
                      "eBWsh00eZsWMCIkrKOuzXW5J89vji55JIxZqbQIDAQABAoIBAEk+2DJs7C/V/USX\n" +
                      "ki8p4ZspR+6V7Y2kW2fjSMdz80INidDiPyxvF5j1xEwrXUZ9sDN5hjN8JISZqoL8\n" +
                      "AxVP9zDjo2qh2qnjIMw830dX43tjyhaE+guXlsojz/PxIvv3BbJcvoSescUwpxta\n" +
                      "x1iP+ZHYyvcKVXKZMX2wDvJBD4NHVCliQWb0+EfB8NfVL7mkw4ak1sRFq+o6B+W8\n" +
                      "Gw2CCJ6Deis3pKFHYi4ASj1rE+8gnK83QTEcHRL9GgDhGEn1zDoXErT+vIUcoxiC\n" +
                      "NoehQQmANIe1DuVQ8uXNzHxshqmysI0EdEIRbXLx6qAlSaLXOSlHQTqx/UI46gwL\n" +
                      "7VVMG/cCgYEA2gagqQ+HBp2XbLG46p+KvAPRI4/fhr2Ar+bDCqu2xmH95L8Je7yf\n" +
                      "OxXLJ7dWP3848spngVPl2NG5ps2fE903aWtNSfObuhsKRxtziqoTbKVcglDqJEpb\n" +
                      "3bkkPQdRB+/HNO5U9xYACihF2Xrj8unw9GSsLAtGVOXTMeMIgRXlL48CgYEAyXt6\n" +
                      "ygCyhsxGIuB5TK5L+bZLy7kQOgeRBx6xrn5XxYagfU/ETa0NMgpWzwwEwiMyqySg\n" +
                      "HbWwOm7tnDNpJyOoeeN4i/p9KDdiXRYvRo+zlm6Db3xdWu5y6vbCk+s0Hj8TFGQ/\n" +
                      "NXBhrQm/6q9+Spi2xQ3K36vrCcaX9mqXL/A5iEMCgYEAitoxs7nJ2rK+32CIThRx\n" +
                      "fLBJn4ZfayaBMIWcrc0SSGHcGrR3y7AjELQegBrI+QODN00kgj72YRGgVNUbfMEk\n" +
                      "KyVFQdW91damwwZ+nU8Xs9fUhMIXfClHPHxO0fc46f8RNWqb/giq2c0wPwN+7ROE\n" +
                      "e4EqDZrYnfUIffsDTYKuRY0CgYABkvVinp7GbtIdc6N+9d2iFcqBzPBTg7ueUp8Q\n" +
                      "vevxnxuh3v2pnbB8s5f6Bh4DZkL/E0os8T5vNm6kycIIjD7EtQI8Fbjkl4otL/8u\n" +
                      "jfDZPAPK2y3fh/1d2I+smCnQEq4TerUDtd0NfQYCz1wtOQQ0hecP2Ef9y49kXXDQ\n" +
                      "7w+EAQKBgAmg5g/bq5J4KgCLdl11AHLNpMfuO4S6TOcHfh65AFhBnBxYFGfMHxLC\n" +
                      "3Ru/0PAiokj1F8bo93bWKzNaTkKtCN+r16LgNw42uZ2a2dhlAf+hRl8xVLXOoUuQ\n" +
                      "AAvqH9uKk0lUMI6lvnmIHvKMBbl47mZ+2FwIcH5fAWWmnlVZhGAN\n" +
                      "-----END RSA PRIVATE KEY-----";

        public MerchantModel(object objectToSign) : base(merchantId, privateKey, apiKey, objectToSign)
        {
        }
    }

    public class OrderMerchantModel : AbstractMerchant
    {
        private static string merchantId = "merchant.test.1";
        private static string apiKey = "MERCHANTTESTKEY";
        private static string privateKey = "-----BEGIN RSA PRIVATE KEY-----\n" +
        "MIIEogIBAAKCAQEA+ODjJAH5gy3LvNn3JaImJkVte/GirW1VylQs07G5SFkcN/3W\n" +
        "zJlosy8lMkHDGmc+CuqSMon4iBuGZwRk/Hyk0fpluvlWisxeWYKPgdwVfx7ebe/s\n" +
        "9Nkcq0JWzS+3k0n7jyb2v68ef22/6svQLvfm5yEELpL0Isen9WcgEZ4hAq01XOdc\n" +
        "WDe3YpZEY1z6u5w32S0gEqEX2og9FCMeG/05Jn8uh6G0Y14hwBW9TxuJ9zf0N1YT\n" +
        "n7i0k13Vwpbnrl0YIxGfx9hj1GuFrrB2/VSZPwyjYiB22r6wG56SrtqtlotPG6wu\n" +
        "spsXHP+4097tvnmWCM/1ignLhNRbrbAkiu2JBQIDAQABAoIBAGTyIm86dc/lRVwG\n" +
        "NJ6JFFw1kODS857QWBnG5p++2F4l9rInPXajSrpTBw1Jay9MbPfUiRQCrlHWiD9E\n" +
        "owleLSQWIikidR27IxG/SSHs+LMk/CcvR7dQULaOI9L55cXSHjzEGxVInGAnTc6Y\n" +
        "eIp0dq2M0LtrU6a/wLpi57j9oHoKpnGN2EPEObcYa4B6ADtaG/jsUdJw4WtwQAQI\n" +
        "G55uNViq01TA82/LDUZETQI4tW2Mvtfy4lk852ER0zurJmbYsV+1lPP080wLEFuX\n" +
        "U7nljUy7TE/EO0831NTERkJrj5R12F+9ChZMcx2FtBtcc9aMZg7nVKreCQKAT7eR\n" +
        "z2/NqokCgYEA+YbLH1QvYXnx3Y644H8qEYgIUUnwVeYF3L1rN1YY3NtawIRS3+uo\n" +
        "1RxYkPJ6twiLu6AqBVDMYbq8nonyXJSIFT8XckIH5MmNcYrRswtT3/lAN6FUithe\n" +
        "JDOccs4nzqs5QgJ0GvbleVwcEiGbKjJCxjNTpKYUPjTh/BcnkTbEY0sCgYEA/1XK\n" +
        "KxZdTTCjmzFtQlgVFhbRdSKdMIyx4SyEF6IlHmE2xDD+H7a52p7BIPk0cd90au63\n" +
        "dxtoIN0gERbMFflPB1EVHighDbgx9Qbsd3d9VPU0P2xF/R5VVgbuWkpFVSTb7wku\n" +
        "Huk81c2/cZozvwzbaW3Y46Oon8dQju7epNdCwu8CgYBIVkpWZ7pgoNB8MV7pPhQ9\n" +
        "BtmabzvsOwXiC0ukvCHzDOhPPU0lOxUzysbmJlVq8GBJ4VGtPtIxVshNkaIgMrMM\n" +
        "rcCnT/sn2UJW8y59oEANgx2ZwvxWUAlip0+ofJlPsZWkzD9Kfd7igtBkdTedUPrI\n" +
        "qKKL7dWafwwwX6oJz/245wKBgDmAcXoInFRw2rdthySEv6Ls2pjdhbbUSI7kPo8v\n" +
        "9doEnJfMTL6JoQtJ60ZouNXYM9iNEAYjxux3v+v5TVa5gYGtUesrsF3tmFqNB53T\n" +
        "7AbA/dY2pQBe32rzPqRYy4pkNZ98WsWdUxT3M+D6kCK7W4I/ubNyPNxWv56gzsUi\n" +
        "sSdDAoGAN0jECzH/MLy4aX2mbg1ulHFAvUK4puyl3IIfP/d6fAttImMDr/gN3irA\n" +
        "WiiGHYK87FnNOPdhPwGKpjXoDYxHlT5ces0V3pGyyf1zotj7kzofJ8d8BXcebiOt\n" +
        "CX+VL6NpdPVSmnNs/1ao15znuhLHGRoK8xzhG+13y58hX2tjbV4=\n" +
        "-----END RSA PRIVATE KEY-----";

        public string BlockChainAddress { get { return "n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx"; } }

        public OrderMerchantModel(object objectToSign) : base(merchantId, privateKey, apiKey, objectToSign)
        {
        }
    }

    public abstract class AbstractMerchant
    {
        public string LykkeMerchantId { get; set; }
        public string LykkeMerchantSign { get; set; }
        public string LykkeMerchantSessionId { get; set; }

        private static string privateKey;
        private static string apiKey;

        public AbstractMerchant(string merchantId, string privateKey, string apiKey, object objectToSign)
        {
            LykkeMerchantId = merchantId;
            AbstractMerchant.privateKey = privateKey;
            AbstractMerchant.apiKey = apiKey;
            LykkeMerchantSign = MERCHANT_SIGN(objectToSign);
        }

        public void Sign(string stringToSign)
        {
            LykkeMerchantSign = MERCHANT_SIGN(stringToSign);
        }

        private static RSA CreateRsaFromPrivateKey(string privateKey)
        {
            privateKey = privateKey.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\n", "");
            var privateKeyBits = System.Convert.FromBase64String(privateKey);

            var RSAparams = new RSAParameters();

            using (BinaryReader binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            return RSA.Create(RSAparams);
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
            if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        private static string MERCHANT_SIGN(object objectToSign)
        {
            var jsonString = objectToSign is string ? objectToSign : JsonConvert.SerializeObject(objectToSign, Formatting.Indented);
            string strToSign = apiKey + jsonString;
            var csp = CreateRsaFromPrivateKey(privateKey);
            var sign = Convert.ToBase64String(csp.SignData(Encoding.UTF8.GetBytes(strToSign), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            return sign;
        }
    }
}
