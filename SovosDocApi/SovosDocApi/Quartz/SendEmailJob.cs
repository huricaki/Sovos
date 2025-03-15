using System.Net.Mail;
using System.Threading.Tasks;
using log4net;
using Quartz;
using SovosDocApi.Repositories.Invoice;

namespace SovosDocApi.Quartz
{
    public class SendEmailJob : IJob
    {
        private readonly ILog myLog = LogManager.GetLogger(typeof(SendEmailJob));
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IConfiguration _configuration;
        public SendEmailJob(IInvoiceRepository invoiceRepository, IConfiguration configuration)
        {
            _invoiceRepository = invoiceRepository;
            _configuration = configuration;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            SendMailRawDoc();
        }

        /// <summary>
        /// İşlenmemiş Faturaları alır ve mail gönderdikten sonra durumlarını günceller
        /// </summary>
        /// <returns></returns>
        public async Task SendMailRawDoc()
        {
            try
            {
                var invoices=(await _invoiceRepository.GetAllInvoicesAsync()).Where(s=>s.IsSendMail==false).ToList();

                foreach (var invoice in invoices) 
                {
                   if(SendMail(invoice))
                   {
                        var updateSendInvoice= invoice;
                        updateSendInvoice.IsSendMail=true;
                        _invoiceRepository.Update(updateSendInvoice);
                       int i=await _invoiceRepository.SaveChangesAsync();
                       if(i>0)
                       {
                            myLog.Info($"{invoice.InvoiceId} idli faturaya ait mail gönderme işlemi tamamlandı, durumu güncellendi.");
                       }
                   }
                }
                                
            }
            catch (Exception ex)
            {
                 myLog.Error($"Fatura maili gönderme jobında hata:",ex);

            }
        } 

        /// <summary>
        /// Gelen  Faturaya ait mail gönderme işlemi yapan method
        /// </summary>
        /// <param name="invoice">işlenmemiş fatura</param>
        /// <returns></returns>
        public bool SendMail(InvoiceHeader invoice)
        {
            try
            {
               var mailtrap= _configuration.GetSection("MailTrap");
                var client = new SmtpClient(mailtrap["host"], Convert.ToInt32(mailtrap["port"]))
                        {       
                            Credentials = new System.Net.NetworkCredential(mailtrap["username"], mailtrap["password"]),
                            EnableSsl = true
                        };

                        client.Send(mailtrap["from"],
                         invoice.Email,
                          $"{invoice.InvoiceLines.Count} kalem ürün içeren {invoice.InvoiceId} nolu faturanız başarıyla işlenmiştir",
                           "Fatura Dökümanı Hk."
                           );

                        return true;
            }
            catch (System.Exception ex)
            {
                myLog.Error($"{invoice.InvoiceId} idli fatura mailinin gönderimi sırasında hata:",ex);
                return false;
            }
        }

    }
}
