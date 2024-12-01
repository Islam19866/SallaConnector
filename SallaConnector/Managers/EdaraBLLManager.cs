using Newtonsoft.Json;
using RestSharp;
using SallaConnector.Context;
using SallaConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Managers
{
    public class EdaraBLLManager
    {


        public static IRestResponse createCustomer(SallaCustomerDTO sallaCustomer, SallaAccount edaraAccount, CustomerAddress customerAddress =null)
        {
           
             CustomerDTO edaraCustomer = new CustomerDTO();
        // mapping
                    edaraCustomer.name = sallaCustomer.first_name + " " + sallaCustomer.last_name;
                    edaraCustomer.code = sallaCustomer.id.ToString();
                    edaraCustomer.mobile = sallaCustomer.mobile.ToString();
                    edaraCustomer.email = sallaCustomer.email;
                    edaraCustomer.customer_type = "Business";
                    edaraCustomer.pricing_type = "EndUser";
                    edaraCustomer.payment_type = "Credit";
                    if (customerAddress != null)
                    {
                       List<CustomerAddress> customerAddresses = new List<CustomerAddress>();
                        customerAddresses.Add(customerAddress);
                        edaraCustomer.customer_addresses = customerAddresses;

                    }
                    return   EdaraIntegration.PostCustomer(edaraCustomer, edaraAccount);
                    //return JsonConvert.SerializeObject(result);

        }

        public static IRestResponse updateCustomer(int edaraCustomerId ,SallaCustomerDTO sallaCustomer, SallaAccount edaraAccount, CustomerAddress customerAddress = null)
        {

            CustomerDTO edaraCustomer = new CustomerDTO();
            // mapping
            edaraCustomer.id = edaraCustomerId;
            edaraCustomer.name = sallaCustomer.first_name + " " + sallaCustomer.last_name;
            edaraCustomer.code = sallaCustomer.id.ToString();
            edaraCustomer.mobile = sallaCustomer.mobile.ToString();
            edaraCustomer.email = sallaCustomer.email;
            edaraCustomer.customer_type = "Business";
            edaraCustomer.pricing_type = "EndUser";
            edaraCustomer.payment_type = "Credit";

            if (customerAddress != null)
            {
                List<CustomerAddress> customerAddresses = new List<CustomerAddress>();
                customerAddresses.Add(customerAddress);
                edaraCustomer.customer_addresses = customerAddresses;

            }
            return EdaraIntegration.updateCustomer(edaraCustomer, edaraAccount);
            //return JsonConvert.SerializeObject(result);

        }

        public static IRestResponse createSalesOrder(SallaSalesOrderDTO sallaSO, SallaAccount edaraAccount)
        {
            
            SalesOrderDTO edaraSO = new SalesOrderDTO();

            edaraSO.customer_id = EdaraIntegration.GetCustomerId(sallaSO.customer.id.ToString(), edaraAccount);

            if (edaraSO.customer_id == 0)
            {

                CustomerAddress customerAddress = mapCustomerAddress(edaraSO.customer_id, edaraAccount, sallaSO.shipping.address);
                createCustomer(sallaSO.customer, edaraAccount, customerAddress);
                edaraSO.customer_id = EdaraIntegration.GetCustomerId(sallaSO.customer.id.ToString(), edaraAccount);
            }
            else
            {
                CustomerAddress customerAddress = mapCustomerAddress(edaraSO.customer_id, edaraAccount, sallaSO.shipping.address);
                updateCustomer(edaraSO.customer_id, sallaSO.customer, edaraAccount, customerAddress);
            }
            edaraSO.external_id = sallaSO.reference_id.ToString();
            edaraSO.paper_number = sallaSO.reference_id.ToString();
            edaraSO.currency_id = EdaraIntegration.GetCurrencyId(sallaSO.currency, edaraAccount);
            edaraSO.taxable = true;
            edaraSO.document_date = DateTime.Parse(sallaSO.date.date.ToString());
            edaraSO.salesstore_id = edaraAccount.EdaraStoreId.Value;
            edaraSO.document_type = "SO";
            edaraSO.channel = "Injaz-" + edaraAccount.SallaMerchantId;
            edaraSO.discount = calcDiscount(sallaSO.amounts.discounts);
            List<SalesOrderDetail> salesOrderDetails = new List<SalesOrderDetail>();

            foreach (var item in sallaSO.items)
            {
                int? tax_id = null;

                if (item.amounts.tax.amount.amount > 0)
                    tax_id = EdaraIntegration.GetTaxId(item.amounts.tax.percent, edaraAccount);

                if (!string.IsNullOrEmpty(item.sku))
                {

                    salesOrderDetails.Add(new SalesOrderDetail
                    {

                        stock_item_id = EdaraIntegration.GetStockItemId(item.sku, edaraAccount),
                        stock_item_description = item.product.description,
                        quantity = item.quantity,
                        price = calcPrice(item.amounts.price_without_tax.amount, double.Parse(item.amounts.tax.percent)),
                        // item_discount = item.amounts.total_discount.amount,
                        warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                        // comments = item.product.name,
                        // item_discount_type = "Value",

                        tax_id = tax_id,

                    });
                }
                else
                {
                    // check bundle
                    salesOrderDetails.Add(new SalesOrderDetail
                    {

                        bundle_id = EdaraIntegration.GetBundleByName(item.name, edaraAccount).FirstOrDefault().bundle_id,
                        stock_item_description = item.product.description,
                        quantity = item.quantity,
                        price = calcPrice(item.amounts.price_without_tax.amount, double.Parse(item.amounts.tax.percent)),
                        // item_discount = item.amounts.total_discount.amount,
                        warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                        //  comments = item.product.name,
                        // item_discount_type = "Value",

                        tax_id = tax_id,

                    });
                }
            }


            // add shipping service
            if (sallaSO.amounts.shipping_cost.amount > 0)
            {
                int? SO_taxid = null;
                if (sallaSO.amounts.tax.amount.amount > 0)
                    SO_taxid = EdaraIntegration.GetTaxId(sallaSO.amounts.tax.percent, edaraAccount);

                salesOrderDetails.Add(new SalesOrderDetail
                {
                    service_item_id = edaraAccount.EdaraShippingServiceId.Value,
                    quantity = 1,
                    price = sallaSO.amounts.shipping_cost.amount + (sallaSO.amounts.shipping_cost.amount * .15),
                    warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                    stock_item_id = null,
                    comments = "Shipping Service",
                    tax_id = SO_taxid
                });
            }

            //"COD Service"
            if (sallaSO.amounts.cash_on_delivery.amount > 0)
            {
                int? SO_taxid = null;

                if (sallaSO.amounts.tax.amount.amount > 0)
                    SO_taxid = EdaraIntegration.GetTaxId(sallaSO.amounts.tax.percent, edaraAccount);

                salesOrderDetails.Add(new SalesOrderDetail
                {
                    service_item_id = edaraAccount.EdaraCODServiceId.Value,
                    quantity = 1,
                    price = sallaSO.amounts.cash_on_delivery.amount + (sallaSO.amounts.cash_on_delivery.amount * .15),
                    warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                    stock_item_id = null,
                    comments = "COD Service",
                    tax_id = SO_taxid
                });
            }
            edaraSO.salesOrder_details = salesOrderDetails;




            var result = EdaraIntegration.PostSalesOrder(edaraSO, edaraAccount);

            // LogManager.LogMessage(JsonConvert.SerializeObject(result.Content));
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                EdaraSOCreateResponse soResult = JsonConvert.DeserializeObject<EdaraSOCreateResponse>(result.Content);
                if (soResult.status_code == 200)
                {
                    LogManager.LogSalesOrderMapping(int.Parse(edaraAccount.SallaMerchantId), sallaSO.reference_id.ToString(), soResult.result, sallaSO.id.ToString());
                    // Payment waiting   , cod 
                    //sallaSO.payment_method
                    string paymentAccount = getPaymentAccount(sallaSO.payment_method, edaraAccount);
                    if (paymentAccount != null)
                    {
                        PaymentDTO paymentDTO = new PaymentDTO()
                        {
                            cash_account_id = sallaSO.payment_method,
                            paid_amount = sallaSO.amounts.total.amount,
                            related_sales_order_code = soResult.result
                        };
                        EdaraIntegration.PostPayment(paymentDTO, edaraAccount);
                    }
                }
            }
            else
            {
                throw new Exception(result.Content);
            }


            return result;

        }

        public static IRestResponse createSalesOrder(SallaEventDTO sallaEvent ,SallaAccount edaraAccount)
        {
            SallaSalesOrderDTO sallaSO = sallaEvent.data.ToObject<SallaSalesOrderDTO>();
            SalesOrderDTO edaraSO = new SalesOrderDTO();

            edaraSO.customer_id = EdaraIntegration.GetCustomerId(sallaSO.customer.id.ToString(), edaraAccount);

            if (edaraSO.customer_id == 0)
            {
                
                CustomerAddress customerAddress = mapCustomerAddress(edaraSO.customer_id,edaraAccount, sallaSO.shipping.address);
                createCustomer(sallaSO.customer, edaraAccount, customerAddress);
                edaraSO.customer_id = EdaraIntegration.GetCustomerId(sallaSO.customer.id.ToString(), edaraAccount);
            }
            else
            {
                CustomerAddress customerAddress = mapCustomerAddress(edaraSO.customer_id,edaraAccount, sallaSO.shipping.address);
                updateCustomer(edaraSO.customer_id,sallaSO.customer, edaraAccount, customerAddress);
            }
            edaraSO.external_id = sallaSO.reference_id.ToString();
            edaraSO.paper_number = sallaSO.reference_id.ToString();
            edaraSO.currency_id = EdaraIntegration.GetCurrencyId(sallaSO.currency, edaraAccount);
            edaraSO.taxable = true;
            edaraSO.document_date =DateTime .Parse(sallaSO.date.date.ToString());
            edaraSO.salesstore_id = edaraAccount.EdaraStoreId.Value;
            edaraSO.document_type = "SO";
            edaraSO.channel = "Injaz-"+sallaEvent.merchant;
            edaraSO.discount = calcDiscount(sallaSO.amounts.discounts);
            List<SalesOrderDetail> salesOrderDetails = new List<SalesOrderDetail>();

            foreach (var item in sallaSO.items)
            {
                int? tax_id = null;

                if (item.amounts.tax.amount.amount > 0)
                    tax_id = EdaraIntegration.GetTaxId(item.amounts.tax.percent, edaraAccount);

                if (!string.IsNullOrEmpty(item.sku))
                {

                    salesOrderDetails.Add(new SalesOrderDetail
                    {

                        stock_item_id = EdaraIntegration.GetStockItemId(item.sku, edaraAccount),
                        stock_item_description = item.product.description,
                        quantity = item.quantity,
                        price = calcPrice(item.amounts.price_without_tax.amount, double.Parse(item.amounts.tax.percent)),
                       // item_discount = item.amounts.total_discount.amount,
                        warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                       // comments = item.product.name,
                        // item_discount_type = "Value",

                        tax_id = tax_id,

                    });
                }
                else
                {
                    // check bundle
                    salesOrderDetails.Add(new SalesOrderDetail
                    {

                        bundle_id = EdaraIntegration.GetBundleByName(item.name, edaraAccount).FirstOrDefault().bundle_id,
                        stock_item_description = item.product.description,
                        quantity = item.quantity,
                        price = calcPrice(item.amounts.price_without_tax.amount, double.Parse(item.amounts.tax.percent)),
                       // item_discount = item.amounts.total_discount.amount,
                        warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                      //  comments = item.product.name,
                        // item_discount_type = "Value",

                        tax_id = tax_id,

                    });
                }
            }

         
            // add shipping service
            if (sallaSO.amounts.shipping_cost.amount > 0)
            {
                int? SO_taxid = null;
                if (sallaSO.amounts.tax.amount.amount > 0)
                    SO_taxid = EdaraIntegration.GetTaxId(sallaSO.amounts.tax.percent, edaraAccount);

                salesOrderDetails.Add(new SalesOrderDetail
                {
                    service_item_id = edaraAccount.EdaraShippingServiceId.Value,
                    quantity = 1,
                    price =sallaSO.amounts.shipping_cost.amount + (sallaSO.amounts.shipping_cost.amount * .15),
                    warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                    stock_item_id = null,
                    comments = "Shipping Service",
                    tax_id = SO_taxid
                });
            }

            //"COD Service"
            if (sallaSO.amounts.cash_on_delivery.amount > 0)
            {
                int? SO_taxid = null;

                if (sallaSO.amounts.tax.amount.amount > 0)
                    SO_taxid = EdaraIntegration.GetTaxId(sallaSO.amounts.tax.percent, edaraAccount);

                salesOrderDetails.Add(new SalesOrderDetail
                {
                    service_item_id = edaraAccount.EdaraCODServiceId.Value,
                    quantity = 1,
                    price = sallaSO.amounts.cash_on_delivery.amount + (sallaSO.amounts.cash_on_delivery.amount * .15),
                    warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                    stock_item_id = null,
                    comments = "COD Service",
                    tax_id = SO_taxid
                });
            }
            edaraSO.salesOrder_details = salesOrderDetails;


            

            var result = EdaraIntegration.PostSalesOrder(edaraSO, edaraAccount);

            // LogManager.LogMessage(JsonConvert.SerializeObject(result.Content));
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                EdaraSOCreateResponse soResult = JsonConvert.DeserializeObject<EdaraSOCreateResponse>(result.Content);
                if (soResult.status_code == 200)
                {
                    LogManager.LogSalesOrderMapping(sallaEvent.merchant, sallaSO.reference_id.ToString(), soResult.result, sallaSO.id.ToString());
                    // Payment waiting   , cod 
                    //sallaSO.payment_method
                    string paymentAccount = getPaymentAccount(sallaSO.payment_method, edaraAccount);
                    if (paymentAccount != null)
                    {
                        PaymentDTO paymentDTO = new PaymentDTO()
                        {
                            cash_account_id = sallaSO.payment_method,
                            paid_amount = sallaSO.amounts.total.amount,
                            related_sales_order_code = soResult.result
                        };
                        EdaraIntegration.PostPayment(paymentDTO, edaraAccount);
                    }
                }
            }
            else
            {
                throw new Exception(result.Content);
            }
            

            return result;

        }

        public static string getPaymentAccount(string PaymentMethod, SallaAccount sallaEdaraAccount)
        {
            switch (PaymentMethod)
            {
                case "cod": return sallaEdaraAccount.PMCOD;
                case "credit":
                    return sallaEdaraAccount.PMCredit;
                case "mada":
                    return sallaEdaraAccount.PMMada;
                case "taby":
                    return sallaEdaraAccount.PMTaby;
                case "tamara":
                    return sallaEdaraAccount.PMTamara;
                case "appleplay":
                    return sallaEdaraAccount.PMApplePay;
                default: return null;
            }
        }

        private static double calcDiscount(List<DiscountDetails> discounts)
        {
            return discounts.Sum(d => d.discount);       
        }

        private static double calcPrice(double priceWithoutTax, double taxPercentage)
        {
            if (taxPercentage > 0)
            {
                return priceWithoutTax + (priceWithoutTax * taxPercentage / 100);
            }
            else
            {
                return priceWithoutTax;
            }
             
        }

        public static IRestResponse updateSalesOrder(SallaEventDTO sallaEvent ,SallaAccount edaraAccount)
        {
             
            SallaSalesOrderDTO sallaSO = sallaEvent.data.ToObject<SallaSalesOrderDTO>();
            SalesOrderDTO edaraSO = new SalesOrderDTO();

            string SO_Code = ConfigManager.getEdaraSOIdBySallaSOId(sallaSO.reference_id.ToString(), sallaEvent.merchant.ToString());
            if (SO_Code == null)
            {
              return  createSalesOrder(sallaEvent,edaraAccount);
            }
            else
            {
                edaraSO.customer_id = EdaraIntegration.GetCustomerId(sallaSO.customer.id.ToString(), edaraAccount);

                if (edaraSO.customer_id == 0)
                {

                    CustomerAddress customerAddress = mapCustomerAddress(edaraSO.customer_id, edaraAccount, sallaSO.shipping.address);
                    createCustomer(sallaSO.customer, edaraAccount, customerAddress);
                    edaraSO.customer_id = EdaraIntegration.GetCustomerId(sallaSO.customer.id.ToString(), edaraAccount);
                }
                else
                {
                    CustomerAddress customerAddress = mapCustomerAddress(edaraSO.customer_id, edaraAccount, sallaSO.shipping.address);
                    updateCustomer(edaraSO.customer_id, sallaSO.customer, edaraAccount, customerAddress);
                }
                //edaraSO.id =
                edaraSO.external_id = sallaSO.reference_id.ToString();
                edaraSO.paper_number = sallaSO.reference_id.ToString();
                edaraSO.currency_id = EdaraIntegration.GetCurrencyId(sallaSO.currency, edaraAccount);
                edaraSO.taxable = true;
                edaraSO.document_date = DateTime.Parse(sallaSO.date.date.ToString());
                edaraSO.salesstore_id = edaraAccount.EdaraStoreId.Value;
                edaraSO.warehouse_id = edaraAccount.EdaraWarehouseId.Value;
                edaraSO.document_type = "SO";
                edaraSO.channel = "Injaz-" + sallaEvent.merchant;
                edaraSO.discount = calcDiscount(sallaSO.amounts.discounts);

                List<SalesOrderDetail> salesOrderDetails = new List<SalesOrderDetail>();

                //foreach (var item in sallaSO.items)
                //{
                //    int? tax_id = null;

                //    if (item.amounts.tax.amount.amount > 0)
                //        tax_id = EdaraIntegration.GetTaxId(item.amounts.tax.percent, edaraAccount);

                //    if (!string.IsNullOrEmpty(item.sku))
                //    {

                //        salesOrderDetails.Add(new SalesOrderDetail
                //        {

                //            stock_item_id = EdaraIntegration.GetStockItemId(item.sku, edaraAccount),
                //            stock_item_description = item.product.description,
                //            quantity = item.quantity,
                //            price = calcPrice(item.amounts.price_without_tax.amount, double.Parse(item.amounts.tax.percent)),
                //            // item_discount = item.amounts.total_discount.amount,
                //            warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                //            comments = item.product.name,
                //            // item_discount_type = "Value",

                //            tax_id = tax_id,

                //        });
                //    }
                //    else
                //    {
                //        // check bundle
                //        salesOrderDetails.Add(new SalesOrderDetail
                //        {

                //            bundle_id = EdaraIntegration.GetBundleByName(item.name, edaraAccount),
                //            stock_item_description = item.product.description,
                //            quantity = item.quantity,
                //            price = calcPrice(item.amounts.price_without_tax.amount, double.Parse(item.amounts.tax.percent)),
                //            // item_discount = item.amounts.total_discount.amount,
                //            warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                //            comments = item.product.name,
                //            // item_discount_type = "Value",

                //            tax_id = tax_id,

                //        });
                //    }
                //}

                foreach (var item in sallaSO.items)
                {
                    int? tax_id = null;

                    if (item.amounts.tax.amount.amount > 0)
                        tax_id = EdaraIntegration.GetTaxId(item.amounts.tax.percent, edaraAccount);

                    if (!string.IsNullOrEmpty(item.sku))
                    {

                        salesOrderDetails.Add(new SalesOrderDetail
                        {

                            stock_item_id = EdaraIntegration.GetStockItemId(item.sku, edaraAccount),
                            stock_item_description = item.product.description,
                            quantity = item.quantity,
                            price = calcPrice(item.amounts.price_without_tax.amount, double.Parse(item.amounts.tax.percent)),
                            // item_discount = item.amounts.total_discount.amount,
                            warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                           // comments = item.product.description,
                            // item_discount_type = "Value",

                            tax_id = tax_id,

                        });
                    }
                    else
                    {
                        List<BundleDetail> bundleDetails = EdaraIntegration.GetBundleByName(item.name, edaraAccount);
                        int bundle_id = bundleDetails.FirstOrDefault().bundle_id;
                        // check bundle
                        //salesOrderDetails.Add(new SalesOrderDetail
                        //{

                        //    bundle_id = EdaraIntegration.GetBundleByName(item.name, edaraAccount),
                        //    stock_item_description = item.product.description,
                        //    quantity = item.quantity,
                        //    price = calcPrice(item.amounts.price_without_tax.amount, double.Parse(item.amounts.tax.percent)),
                        //    item_discount = item.amounts.total_discount.amount,
                        //    warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                        //    comments = item.product.name,
                        //    // item_discount_type = "Value",

                        //    tax_id = tax_id,

                        //});

                        if (bundle_id != 0)
                        {
                            foreach (var bundleItem in item.consisted_products)
                            {
                                var bundleStockItem = GetBundleStockItem(bundleDetails, bundleItem.sku);
                                // add bundle itesm
                                salesOrderDetails.Add(new SalesOrderDetail
                                {

                                    bundle_id = bundle_id,
                                    stock_item_id = bundleStockItem.stock_item_id,
                                    stock_item_description = bundleItem.name,
                                    quantity = bundleStockItem.quantity * item.quantity,
                                    bundle_quantity = item.quantity,
                                  // price = calcBundlePrice(bundleDetails,bundleItem.sku),
                                    price = bundleStockItem.price,
                                    //  item_discount = item.amounts.total_discount.amount/item.consisted_products.Count(),
                                    warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                                    //comments = bundleItem.name,
                                    // item_discount_type = "Value",

                                    tax_id = tax_id,


                                });
                            }
                        }
                    }
                }
                // add shipping service
                if (sallaSO.amounts.shipping_cost.amount > 0)
                {
                    int? SO_taxid = null;
                    if (sallaSO.amounts.tax.amount.amount > 0)
                        SO_taxid = EdaraIntegration.GetTaxId(sallaSO.amounts.tax.percent, edaraAccount);

                    salesOrderDetails.Add(new SalesOrderDetail
                    {
                        service_item_id = edaraAccount.EdaraShippingServiceId.Value,
                        quantity = 1,
                        //price = sallaSO.amounts.shipping_cost.amount + sallaSO.amounts.tax.amount.amount,
                        price = sallaSO.amounts.shipping_cost.amount + (sallaSO.amounts.shipping_cost.amount * .15),

                        warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                        stock_item_id = null,
                        comments = "Shipping Service",
                        tax_id = SO_taxid
                    });
                }

                //"COD Service"
                if (sallaSO.amounts.cash_on_delivery.amount > 0)
                {
                    int? SO_taxid = null;

                    if (sallaSO.amounts.tax.amount.amount > 0)
                        SO_taxid = EdaraIntegration.GetTaxId(sallaSO.amounts.tax.percent, edaraAccount);

                    salesOrderDetails.Add(new SalesOrderDetail
                    {
                        service_item_id = edaraAccount.EdaraCODServiceId.Value,
                        quantity = 1,
                    //    price = sallaSO.amounts.cash_on_delivery.amount,
                        price = sallaSO.amounts.cash_on_delivery.amount + (sallaSO.amounts.cash_on_delivery.amount * .15),

                        warehouse_id = edaraAccount.EdaraWarehouseId.Value,
                        stock_item_id = null,
                        comments = "COD Service",
                        tax_id = SO_taxid
                    });
                }
                edaraSO.salesOrder_details = salesOrderDetails;
                var result = EdaraIntegration.UpdateSalesOrder(edaraSO, edaraAccount, SO_Code);
                //sallaSO.payment_method
                //string paymentAccount = getPaymentAccount(sallaSO.payment_method, edaraAccount);
                //if (paymentAccount != null)
                //{
                //    PaymentDTO paymentDTO = new PaymentDTO()
                //    {
                //        cash_account_id = sallaSO.payment_method,
                //        paid_amount = sallaSO.amounts.total.amount,
                //        related_sales_order_code = result.re.result
                //    };
                //    EdaraIntegration.PostPayment(paymentDTO, edaraAccount);
                //}
                // LogManager.LogMessage(JsonConvert.SerializeObject(result.Content));
                return result;
            }
        }


        public static List<RequestLog> Getlogs()
        {


            //using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            //{
            //    return db.RequestLogs.Where(e=>e.EventDetails.Contains(keyword) | e.Payload.Contains(keyword)).ToList();
            //}
            DateTime specificDate = DateTime.Now.AddDays(-5);
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
                return db.RequestLogs.Where(l=> l.EventType=="order.created" |  l.EventType == "order.updated").Where(l=> l.ResponseStatus != "OK" & l.RequestDate > specificDate).ToList();
               

            }
        }

        public static RequestLog GetlogDetails(int id)
        {
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
                return db.RequestLogs.Find(id);
            }
        }
        private static BundleDetail GetBundleStockItem(List<BundleDetail> bundleDetails, string sku)
        {
            return bundleDetails.Where(b => b.stock_item_code == sku).FirstOrDefault();
        }

        private static CustomerAddress mapCustomerAddress(int customerId, SallaAccount edaraAccount, Address address)
        {
            // check customer address
            CustomerAddress customerAddress = new CustomerAddress();


            if (address.country == null | address.city == null | address.block == null)
                return null;

           var currentAddress =  EdaraIntegration.GetCustomerAddress(customerId.ToString(),edaraAccount);
            if (currentAddress.result != null)
            {
                customerAddress.id = currentAddress.result.FirstOrDefault().id;
                customerAddress.entity_state = "modified";
            }
          var city = EdaraIntegration.GetCityByName(address.city.ToString(), edaraAccount);
            if (city.result == null)
            {
                EdaraIntegration.CreateCity(new EdaraCityCreateRequest() { name = address.city.ToString(), country_id = 2 }, edaraAccount);
                  city = EdaraIntegration.GetCityByName(address.city.ToString(), edaraAccount);

            }

           var district = EdaraIntegration.GetDistrictName(address.block.ToString(), edaraAccount);
            if (district.result == null)
            {
                EdaraIntegration.CreateDistrict(new EdaraDistrictCreateRequest() { name = address.block.ToString(), city_id = city.result.id }, edaraAccount);
                  district = EdaraIntegration.GetDistrictName(address.block.ToString(), edaraAccount);
            }


            customerAddress.street = address.street_number.ToString() + " " + address.shipping_address.ToString();
            customerAddress.city_id = city.result.id;
            customerAddress.country_id = city.result.country_id;
            customerAddress.district_id = district.result.id;
            customerAddress.customer_id = customerId;
            customerAddress.is_default = true;

            return customerAddress;
        }
    }
}