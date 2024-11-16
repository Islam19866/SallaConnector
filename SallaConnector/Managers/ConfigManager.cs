using SallaConnector.Context;
using SallaConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Managers
{
    public class ConfigManager
    {
        //    public static int GetShippingServiceId ()
        //    {
        //    string id = System.Configuration.ConfigurationManager.AppSettings["edaraShippingServiceId"].ToString();
        //        if (!String.IsNullOrEmpty(id))
        //            return int.Parse(id);
        //        else
        //        return 0 ;
        //}
        //    public static int GetTaxId()
        //    {
        //        string id = System.Configuration.ConfigurationManager.AppSettings["taxId"].ToString();
        //        if (!String.IsNullOrEmpty(id))
        //            return int.Parse(id);
        //        else
        //            return 0;
        //    }
        //    public static int GetEdaraIntgBaseUrld()
        //    {
        //        string id = System.Configuration.ConfigurationManager.AppSettings["integrBaseUrl"].ToString();
        //        if (!String.IsNullOrEmpty(id))
        //            return int.Parse(id);
        //        else
        //            return 0;
        //    }

        public static SettingValidationResponse updateSallaSettings(SallaEventDTO sallaEvent)
        {
            SettingValidationResponse settingValidationResponse = new SettingValidationResponse();
            SallaAppSettingsDTO sallaSetting = sallaEvent.data.ToObject<SallaAppSettingsDTO>();
            
            try
            {


                using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
                {
                    var sallaStoreindb = db.SallaAccounts.Where(a => a.SallaMerchantId == sallaEvent.merchant.ToString()).FirstOrDefault();
                    if (sallaStoreindb != null)
                    {
                           sallaStoreindb.EdaraToken = sallaSetting.EdaraToken;

                           EdaraIntegration.CheckEdaraUser(sallaSetting.UserName, sallaStoreindb);
                        
                            sallaStoreindb.EdaraEmail = sallaSetting.email;
                            sallaStoreindb.IsActive = true;
                            sallaStoreindb.Email = sallaSetting.email;
                            sallaStoreindb.EdaraUser = sallaSetting.UserName;
                            sallaStoreindb.EdaraTenantName = sallaSetting.EdaraURL.Replace("https://", "").Split('.').FirstOrDefault();
                            sallaStoreindb.EdaraStoreId = EdaraIntegration.GetStoredByName(sallaSetting.SalesStoreName, sallaStoreindb);
                            sallaStoreindb.EdaraWarehouseId = EdaraIntegration.GetWarehouseIdByName(sallaSetting.WarehouseName, sallaStoreindb);
                            sallaStoreindb.EdaraShippingServiceId = EdaraIntegration.GetServiceItemId(sallaSetting.ShippingServiceName, sallaStoreindb);
                            sallaStoreindb.EdaraCODServiceId = EdaraIntegration.GetServiceItemId(sallaSetting.CODService, sallaStoreindb);

                        //db.StatusMappings.Add(new StatusMapping() {
                        //    SallaMerchantId = sallaEvent.merchant.ToString(),
                        //    SallaStatusName = sallaSetting.ShippingStatus.ToString(),
                        //    EdaraStatusName = ""
                        //});

                        //db.StatusMappings.Add(new StatusMapping()
                        //{
                        //    SallaMerchantId = sallaEvent.merchant.ToString(),
                        //    SallaStatusName = sallaSetting.InProgressStatus.ToString(),
                        //    EdaraStatusName = ""
                        //});
                        sallaStoreindb.SallaShippedStatusId = sallaSetting.ShippingStatus;
                            sallaStoreindb.SallaInProgressStatusId = sallaSetting.InProgressStatus;

                        sallaStoreindb.EdaraPriceListId = sallaSetting.PriceListId;
                            db.SaveChanges();
                        
                    }
                    else
                    {

                        throw new Exception("Store not installed ");

                    }
                }
                settingValidationResponse.status = 200;
                settingValidationResponse.success = true;
                settingValidationResponse.code = "";
                settingValidationResponse.message ="";
                return settingValidationResponse;
            }
            catch (Exception ex)
            {
                settingValidationResponse.status = 442;
                settingValidationResponse.success = false;
                settingValidationResponse.code = "error";
                settingValidationResponse.message = ex.Message;

                return settingValidationResponse;



            }

        }
        public static bool addSallaAccount(SallaEventDTO sallaEvent)
        {
            SallaAuthorizeResponse sallaStore = sallaEvent.data.ToObject<SallaAuthorizeResponse>();
            try
            {


                using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
                {
                    var sallaStoreindb = db.SallaAccounts.Where(a => a.SallaMerchantId == sallaEvent.merchant.ToString()).FirstOrDefault();
                    if (sallaStoreindb != null)
                    {
                        sallaStoreindb.SallaToken = sallaStore.access_token;
                        sallaStoreindb.SallaRefreshToken = sallaStore.refresh_token;
                        sallaStoreindb.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                    }
                    else
                    {
                        SallaAccount sallaAccount = new SallaAccount();
                        sallaAccount.SallaMerchantId = sallaEvent.merchant.ToString();
                        sallaAccount.SallaToken = sallaStore.access_token;
                        sallaAccount.SallaRefreshToken = sallaStore.refresh_token;
                        sallaAccount.SallaStoreName = sallaStore.app_name;
                        sallaAccount.CreatedDate = DateTime.Now;
                        db.SallaAccounts.Add(sallaAccount);
                        db.SaveChanges();

                       
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogManager.LogMessage(ex.Message, "Exception");
                return false;
                
            }

        }

        public static string getEdaraSOIdBySallaSOId(string SalesOrderId, string merchantId)
        {

            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
                var result = db.SalesOrdersMappings.Where(a => a.SourceSalesId == SalesOrderId & a.MerchantId==merchantId).FirstOrDefault();
                if (result != null)
                {
                    return result.DestinationSalesId;

                }
                return null;
            }
        }

        public static string geSallaInternalId(string edaraOrderNo, string merchantId)
        {

            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
                var result = db.SalesOrdersMappings.Where(a => a.DestinationSalesId == edaraOrderNo & a.MerchantId == merchantId).FirstOrDefault();
                if (result != null)
                {
                    return result.SourceInternalId;

                }
                return null;
            }
        }


        public static int getMappedSallaStatus(string merchantId, string statusName)
        {

            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
                var sallaStore = db.SallaAccounts.Where(a => a.SallaMerchantId == merchantId & a.IsActive).FirstOrDefault();
                if (sallaStore != null)
                {
                    if (statusName == "Processing")
                        return sallaStore.SallaInProgressStatusId.Value;
                    if (statusName == "Shipeped")
                        return sallaStore.SallaShippedStatusId.Value;
                    
                }
                return 0;
            }
        }

        public static List<SallaAccount> getActiveStoresByWarehouse(string tenantName, int warehouseId)
        {
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
                return db.SallaAccounts.Where(a => a.EdaraTenantName == tenantName & a.IsActive & a.EdaraWarehouseId==warehouseId).ToList();
            }
        }

        public static List<SallaAccount> getActiveStoreBySalesStore(string tenantName, int salesStoreId)
        {
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
               return db.SallaAccounts.Where(a => a.EdaraTenantName == tenantName & a.IsActive & a.EdaraStoreId== salesStoreId).ToList();
            }
        }

        public static SallaAccount getLinkedEdara(int merchantId)
        {
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
                return db.SallaAccounts.Where(a => a.SallaMerchantId == merchantId.ToString()  & a.IsActive).FirstOrDefault();
            }
        }

        public static List<SallaAccount> getActiveStores(string tenantName)
        {
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
                return db.SallaAccounts.Where(a => a.EdaraTenantName == tenantName & a.IsActive).ToList();
            }
        }
    }
}