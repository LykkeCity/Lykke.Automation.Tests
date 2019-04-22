using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace XUnitTestCommon.TestCreator
{
    public class RequestModel
    {
        public string RequestModelURL { get; set; }
        public string summary { get; set; }

        public List<ApiMethodModel> ApiMethods { get; set; }

        public RequestModel()
        {
        }
    }

    public class ApiMethodModel
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("parameters")]
        public List<ParametersModel> ParametersModel { get; set; }

        [JsonProperty("responses")]
        public Responses Responses { get; set; }

        public string ApiMethod { get; set; }
    }

    public class ParametersModel
    {
        public string name { get; set; }
        public string _in { get; set; }
        public string description { get; set; }
        public bool required { get; set; }
        public string type { get; set; }
    }

    public class Responses
    {
        private List<string> result = new List<string>();
        #region DefaultResponseModels
        private DefaultResponseModel __200;
        private DefaultResponseModel __201;
        private DefaultResponseModel __202;
        private DefaultResponseModel __203;
        private DefaultResponseModel __204;
        private DefaultResponseModel __205;
        private DefaultResponseModel __206;
        private DefaultResponseModel __207;
        private DefaultResponseModel __208;
        private DefaultResponseModel __226;

        private DefaultResponseModel __400;
        private DefaultResponseModel __401;
        private DefaultResponseModel __402;
        private DefaultResponseModel __403;
        private DefaultResponseModel __404;
        private DefaultResponseModel __405;
        private DefaultResponseModel __406;
        private DefaultResponseModel __407;
        private DefaultResponseModel __408;
        private DefaultResponseModel __409;
        private DefaultResponseModel __410;
        private DefaultResponseModel __411;
        private DefaultResponseModel __412;
        private DefaultResponseModel __413;
        private DefaultResponseModel __414;
        private DefaultResponseModel __415;
        private DefaultResponseModel __416;
        private DefaultResponseModel __417;
        private DefaultResponseModel __418;
        private DefaultResponseModel __421;
        private DefaultResponseModel __422;
        private DefaultResponseModel __423;
        private DefaultResponseModel __424;
        private DefaultResponseModel __426;
        private DefaultResponseModel __428;
        private DefaultResponseModel __429;
        private DefaultResponseModel __431;
        private DefaultResponseModel __449;
        private DefaultResponseModel __451;

        private DefaultResponseModel __500;
        private DefaultResponseModel __501;
        private DefaultResponseModel __502;
        private DefaultResponseModel __503;
        private DefaultResponseModel __504;
        private DefaultResponseModel __505;
        private DefaultResponseModel __506;
        private DefaultResponseModel __507;
        private DefaultResponseModel __508;
        private DefaultResponseModel __509;
        private DefaultResponseModel __510;
        private DefaultResponseModel __511;
        private DefaultResponseModel __512;
        private DefaultResponseModel __513;
        private DefaultResponseModel __514;
        private DefaultResponseModel __515;
        private DefaultResponseModel __516;
        private DefaultResponseModel __517;
        private DefaultResponseModel __518;
        private DefaultResponseModel __519;
        private DefaultResponseModel __520;
        #endregion


        [JsonProperty("200")]
        public DefaultResponseModel _200 { get { return __200; } set { __200 = value;/* result.Add("200");*/ } }
        [JsonProperty("201")]
        public DefaultResponseModel _201 { get { return __201; } set { __201 = value; result.Add("201"); } }
        [JsonProperty("202")]
        public DefaultResponseModel _202 { get { return __202; } set { __202 = value; result.Add("202"); } }
        [JsonProperty("203")]
        public DefaultResponseModel _203 { get { return __203; } set { __203 = value; result.Add("203"); } }
        [JsonProperty("204")]
        public DefaultResponseModel _204 { get { return __204; } set { __204 = value; result.Add("204"); } }
        [JsonProperty("205")]
        public DefaultResponseModel _205 { get { return __205; } set { __205 = value; result.Add("205"); } }
        [JsonProperty("206")]
        public DefaultResponseModel _206 { get { return __206; } set { __206 = value; result.Add("206"); } }
        [JsonProperty("207")]
        public DefaultResponseModel _207 { get { return __207; } set { __207 = value; result.Add("207"); } }
        [JsonProperty("208")]
        public DefaultResponseModel _208 { get { return __208; } set { __208 = value; result.Add("208"); } }
        [JsonProperty("226")]
        public DefaultResponseModel _226 { get { return __226; } set { __226 = value; result.Add("226"); } }


        [JsonProperty("400")]
        public DefaultResponseModel _400 { get { return __400; } set { __400 = value; result.Add("400"); } }
        [JsonProperty("401")]
        public DefaultResponseModel _401 { get { return __401; } set { __401 = value; result.Add("401"); } }
        [JsonProperty("402")]
        public DefaultResponseModel _402 { get { return __402; } set { __402 = value; result.Add("402"); } }
        [JsonProperty("403")]
        public DefaultResponseModel _403 { get { return __403; } set { __403 = value; result.Add("403"); } }
        [JsonProperty("404")]
        public DefaultResponseModel _404 { get { return __404; } set { __404 = value; result.Add("404"); } }
        [JsonProperty("405")]
        public DefaultResponseModel _405 { get { return __405; } set { __405 = value; result.Add("405"); } }
        [JsonProperty("406")]
        public DefaultResponseModel _406 { get { return __406; } set { __406 = value; result.Add("406"); } }
        [JsonProperty("407")]
        public DefaultResponseModel _407 { get { return __407; } set { __407 = value; result.Add("407"); } }
        [JsonProperty("408")]
        public DefaultResponseModel _408 { get { return __408; } set { __408 = value; result.Add("408"); } }
        [JsonProperty("409")]
        public DefaultResponseModel _409 { get { return __409; } set { __409 = value; result.Add("409"); } }
        [JsonProperty("410")]
        public DefaultResponseModel _410 { get { return __410; } set { __410 = value; result.Add("410"); } }
        [JsonProperty("411")]
        public DefaultResponseModel _411 { get { return __411; } set { __411 = value; result.Add("411"); } }
        [JsonProperty("412")]
        public DefaultResponseModel _412 { get { return __412; } set { __412 = value; result.Add("412"); } }
        [JsonProperty("413")]
        public DefaultResponseModel _413 { get { return __413; } set { __413 = value; result.Add("413"); } }
        [JsonProperty("414")]
        public DefaultResponseModel _414 { get { return __414; } set { __414 = value; result.Add("414"); } }
        [JsonProperty("415")]
        public DefaultResponseModel _415 { get { return __415; } set { __415 = value; result.Add("415"); } }
        [JsonProperty("416")]
        public DefaultResponseModel _416 { get { return __416; } set { __416 = value; result.Add("416"); } }
        [JsonProperty("417")]
        public DefaultResponseModel _417 { get { return __417; } set { __417 = value; result.Add("417"); } }
        [JsonProperty("418")]
        public DefaultResponseModel _418 { get { return __418; } set { __418 = value; result.Add("418"); } }
        [JsonProperty("421")]
        public DefaultResponseModel _421 { get { return __421; } set { __421 = value; result.Add("421"); } }
        [JsonProperty("422")]
        public DefaultResponseModel _422 { get { return __422; } set { __422 = value; result.Add("422"); } }
        [JsonProperty("423")]
        public DefaultResponseModel _423 { get { return __423; } set { __423 = value; result.Add("423"); } }
        [JsonProperty("424")]
        public DefaultResponseModel _424 { get { return __424; } set { __424 = value; result.Add("424"); } }
        [JsonProperty("426")]
        public DefaultResponseModel _426 { get { return __426; } set { __426 = value; result.Add("426"); } }
        [JsonProperty("428")]
        public DefaultResponseModel _428 { get { return __428; } set { __428 = value; result.Add("428"); } }
        [JsonProperty("429")]
        public DefaultResponseModel _429 { get { return __429; } set { __429 = value; result.Add("429"); } }
        [JsonProperty("431")]
        public DefaultResponseModel _431 { get { return __431; } set { __431 = value; result.Add("431"); } }
        [JsonProperty("449")]
        public DefaultResponseModel _449 { get { return __449; } set { __449 = value; result.Add("449"); } }
        [JsonProperty("451")]
        public DefaultResponseModel _451 { get { return __451; } set { __451 = value; result.Add("451"); } }

        [JsonProperty("500")]
        public DefaultResponseModel _500 { get { return __500; } set { __500 = value; result.Add("500"); } }
        [JsonProperty("501")]
        public DefaultResponseModel _501 { get { return __501; } set { __501 = value; result.Add("501"); } }
        [JsonProperty("502")]
        public DefaultResponseModel _502 { get { return __502; } set { __502 = value; result.Add("502"); } }
        [JsonProperty("503")]
        public DefaultResponseModel _503 { get { return __503; } set { __503 = value; result.Add("503"); } }
        [JsonProperty("504")]
        public DefaultResponseModel _504 { get { return __504; } set { __504 = value; result.Add("504"); } }
        [JsonProperty("505")]
        public DefaultResponseModel _505 { get { return __505; } set { __505 = value; result.Add("505"); } }
        [JsonProperty("506")]
        public DefaultResponseModel _506 { get { return __506; } set { __506 = value; result.Add("506"); } }
        [JsonProperty("507")]
        public DefaultResponseModel _507 { get { return __507; } set { __507 = value; result.Add("507"); } }
        [JsonProperty("508")]
        public DefaultResponseModel _508 { get { return __508; } set { __508 = value; result.Add("508"); } }
        [JsonProperty("509")]
        public DefaultResponseModel _509 { get { return __509; } set { __509 = value; result.Add("509"); } }
        [JsonProperty("510")]
        public DefaultResponseModel _510 { get { return __510; } set { __510 = value; result.Add("510"); } }
        [JsonProperty("511")]
        public DefaultResponseModel _511 { get { return __511; } set { __511 = value; result.Add("511"); } }
        [JsonProperty("512")]
        public DefaultResponseModel _512 { get { return __512; } set { __512 = value; result.Add("512"); } }
        [JsonProperty("513")]
        public DefaultResponseModel _513 { get { return __513; } set { __513 = value; result.Add("513"); } }
        [JsonProperty("514")]
        public DefaultResponseModel _514 { get { return __514; } set { __514 = value; result.Add("514"); } }
        [JsonProperty("515")]
        public DefaultResponseModel _515 { get { return __515; } set { __515 = value; result.Add("515"); } }
        [JsonProperty("516")]
        public DefaultResponseModel _516 { get { return __516; } set { __516 = value; result.Add("516"); } }
        [JsonProperty("517")]
        public DefaultResponseModel _517 { get { return __517; } set { __517 = value; result.Add("517"); } }
        [JsonProperty("518")]
        public DefaultResponseModel _518 { get { return __518; } set { __518 = value; result.Add("518"); } }
        [JsonProperty("519")]
        public DefaultResponseModel _519 { get { return __519; } set { __519 = value; result.Add("519"); } }

        public List<string> GetResponses()
        {
            return result;
        }
    }

    #region status codes models
    public class DefaultResponseModel
    {
        public string description { get; set; }
        public Schema schema { get; set; }
    }

    #endregion
    #region responses models
    // Responses models
    public class Schema
    {
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Properties
    {
        public Id id { get; set; }
        public Name name { get; set; }
    }

    public class Id
    {
        public string type { get; set; }
        public int example { get; set; }
    }

    public class Name
    {
        public string type { get; set; }
        public string example { get; set; }
    }
    #endregion

}
