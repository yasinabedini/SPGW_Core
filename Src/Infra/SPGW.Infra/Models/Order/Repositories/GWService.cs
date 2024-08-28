using SPGW.Domain.Common;
using SPGW.Domain.Customer.Entities;
using SPGW.Domain.DTOs;
using SPGW.Domain.Order.Repositories;
using SPGW.Domain.Psp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Infra.Models.Order.Repositories
{
    public class GWService : IGWService
    {
        private IEFRepository<Domain.Customer.Entities.Customer> _customerRepository;
        private IEFRepository<Domain.Order.Entities.Order> _orderRepository;
        public GWService(
            IEFRepository<Domain.Customer.Entities.Customer> customerRepository,
            IEFRepository<Domain.Order.Entities.Order> orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        public ServiceResult<PayOrderModel> GetOrderInformation(string orderCode)
        {
            var result = new ServiceResult<PayOrderModel>();
            try
            {
                if (string.IsNullOrEmpty(orderCode))
                {
                    result.ErrorCode = 7;
                    result.ErrorMessage = "کد سفارش ارسال شده نا معتبر است";
                }
                else
                {
                    var currentOrder = _orderRepository.Queryable().FirstOrDefault(o => o.OrderCode == orderCode);
                    if (currentOrder == null)
                    {
                        result.ErrorCode = 7;
                        result.ErrorMessage = "کد سفارش ارسال شده نا معتبر است";
                    }
                    else
                    {
                        if (!currentOrder.Customer.IsActive)
                        {
                            result.ErrorCode = 8;
                            result.ErrorMessage = "مشتری " + currentOrder.Customer.ShopName + " غیر فعال است. لطفا با پشتیبانی تماس بگیرید";
                        }
                        else
                        {
                            var merchants = currentOrder.Customer.Merchants.Where(m => m.IsActive);
                            if (!merchants.Any())
                            {
                                result.ErrorCode = 9;
                                result.ErrorMessage = "مشتری " + currentOrder.Customer.ShopName + " هیچ پذیرنده فعالی ندارد. لطفا با پشتیبانی تماس بگیرید";
                            }
                            else
                            {
                                var psps = merchants.Where(m => m.PSP.IsActive);
                                if (!psps.Any())
                                {
                                    result.ErrorCode = 10;
                                    result.ErrorMessage = "مشتری " + currentOrder.Customer.ShopName + " هیچ درگاه فعالی ندارد. لطفا با پشتیبانی تماس بگیرید";
                                }
                                else
                                {
                                    result.Result = new PayOrderModel
                                    {
                                        Amount = currentOrder.Amount,
                                        ShopName = currentOrder.Customer.ShopName,
                                        ShopAddress = currentOrder.Customer.WebSiteAddress,
                                        InvoiceDate = currentOrder.InvoiceDate,
                                        InvoiceNumber = currentOrder.InvoiceNumber,
                                        TotalAmount = currentOrder.TotalAmount,
                                        RedirectAddress = currentOrder.RedirectAddress,
                                        OrderCode = currentOrder.OrderCode,

                                        GateWays = psps.Select(m => (PSPs)m.PSPId).ToArray(),

                                        Logo = currentOrder.Customer.Logo,

                                        DirectPSPGW = currentOrder.DirectPSPGW,
                                        PSP = (Nullable<PSPs>)currentOrder.PSPCode
                                    };
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                throw;
            }
            return result;
        }
    }
}
