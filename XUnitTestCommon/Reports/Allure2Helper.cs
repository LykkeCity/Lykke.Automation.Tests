using Allure.Commons;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Tests;

namespace XUnitTestCommon.TestsCore
{
    public static class Allure2Helper
    {
        private static AllureLifecycle allure => AllureLifecycle.Instance;
        private static ConcurrentDictionary<string, List<Attachment>> attachmens = new ConcurrentDictionary<string, List<Attachment>>();

        public static void UpdateStep(List<Attachment> attachments)
        {
            allure.UpdateStep(x => { new StepResult
            {
                attachments = attachments
            }; });
        }

        public static void UpdateStep(Attachment attachment)
        {
            allure.UpdateStep(x => {
                new StepResult
                {
                    attachments = new List<Attachment> { attachment }
                };
            });
        }

        public static void AttachText(string name, string text) => MakeTextAttach(name, text, "text/plain");

        public static void AttachJson(string name, string json) => MakeTextAttach(name, json, "application/json");

        public static void AttachXml(string name, string xml) => MakeTextAttach(name, xml, "text/xml");

        public static void AttachHtml(string name, string html) => MakeTextAttach(name, html, "text/html");

        public static void AttachPng(string name, byte[] pngData) => MakeBinaryAttach(name, pngData, "image/png");

        private static void MakeTextAttach(string name, string context, string type)
        {
            string fileExtension = type == "text/plain" ? ".txt" :
                                   type == "application/json" ? ".json" :
                                   type == "text/xml" ? ".xml" :
                                   type == "text/html" ? ".html" :
                                   ".attach";

            string attachFile = Guid.NewGuid().ToString("N") + fileExtension;

            try
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(allure.ResultsDirectory, attachFile), context);
            }
            catch (Exception)
            {
                return;
            }

            AddAttachment(name, type, attachFile);
        }

        private static void MakeBinaryAttach(string name, byte[] context, string type)
        {
            string fileExtension = type == "image/png" ? ".png" :
                                   ".bin";

            string attachFile = Guid.NewGuid().ToString("N") + fileExtension;

            try
            {
                System.IO.File.WriteAllBytes(System.IO.Path.Combine(allure.ResultsDirectory, attachFile), context);
            }
            catch (Exception)
            {
                return;
            }

            AddAttachment(name, type, attachFile);
        }

        private static void AddAttachment(string name, string type, string attachFile)
        {
            var attach = new Attachment()
            {
                name = name,
                source = attachFile,
                type = type
            };

            string caseId = TestContext.CurrentContext.Test.ID;
            if (BaseTest.isStepOpened.Value > 0)
            {
                if(BaseTest.attaches.Value.ContainsKey(BaseTest.stepUID.Value.Peek()))
                    BaseTest.attaches.Value[BaseTest.stepUID.Value.Peek()].Add(attach);
                else
                    BaseTest.attaches.Value.Add(BaseTest.stepUID.Value.Peek(), new List<Attachment> { attach });
            }
            else
                attachmens.AddOrUpdate(caseId, new List<Attachment> { attach }, (k, v) => { v.Add(attach); return v; });
        }

        public static List<Attachment> GetAttaches()
        {
            string caseId = TestContext.CurrentContext.Test.ID;
            List<Attachment> attaches;
            attachmens.TryRemove(caseId, out attaches);

            return attaches ?? new List<Attachment>();
        }
    }
}
