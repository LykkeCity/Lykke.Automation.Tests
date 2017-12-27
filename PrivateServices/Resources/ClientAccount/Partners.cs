using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class Partners : ClientAccountBase
    {
        public IResponse<List<Partner>> GetPartners()
        {
            return Request.Get("/api/Partners").Build().Execute<List<Partner>>();
        }

        public IResponse PostPartners(Partner partner)
        {
            return Request.Post("/api/Partners").AddJsonBody(partner).Build().Execute();
        }

        public IResponse PutPartners(Partner partner)
        {
            return Request.Put("/api/Partners").AddJsonBody(partner).Build().Execute();
        }

        public IResponse<List<Partner>> GetPartnersAll()
        {
            return Request.Get("/api/Partners/all").Build().Execute<List<Partner>>();
        }

        public IResponse<List<Partner>> PostPartnersByPublicIds(List<string> publicIds)
        {
            return Request.Post("/api/Partners/filterByPublicIds").AddJsonBody(publicIds)
                .Build().Execute<List<Partner>>();
        }

        public IResponse<List<Partner>> PostPartnersByIds(List<string> internalIds)
        {
            return Request.Post("/api/Partners/filterByIds").AddJsonBody(internalIds)
                .Build().Execute<List<Partner>>();
        }

        public IResponse<Partner> GetPartnerById(string internalId)
        {
            return Request.Get($"/api/Partners/{internalId}").Build().Execute<Partner>();
        }

        //TODO: internal?
        public IResponse<ClientCountResponseModel> GetPartnerUserCount(string publicId)
        {
            return Request.Get("/api/Partners/getUsersCount")
                .AddQueryParameter("partnerId", publicId).Build().Execute<ClientCountResponseModel>();
        }

        public IResponse DeleteRemovePartner(string partnerInternalId)
        {
            return Request.Delete("/api/Partners/removePartner").AddQueryParameter("partnerInternalId", partnerInternalId)
                .Build().Execute();
        }

        public IResponse PostUpdatePartner(Partner partner)
        {
            return Request.Post("/api/Partners/updatePartner").AddJsonBody(partner).Build().Execute();
        }
    }
}
