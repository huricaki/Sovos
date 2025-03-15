using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using SovosDocApi.Models;
using SovosDocApi.Repositories;
using SovosDocApi.Repositories.Invoice;

namespace SovosDocApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private static readonly ILog myLog = LogManager.GetLogger(typeof(InvoiceController));

        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceController(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        /// <summary>
        /// Gelen json objesine ait belgeyi kaydeder
        /// </summary>
        /// <param name="invoiceDto">gönderilen json objesi</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceDto invoiceDto)
        {
            try
            {
                if (invoiceDto == null || invoiceDto.InvoiceHeader == null
                || invoiceDto.InvoiceLines == null || invoiceDto.InvoiceLines.Count == 0)
                {
                    myLog.Error($"Geçersiz belge formatı.. Lütfen json objenizi kontrol ediniz.. Fatura Id'niz: {invoiceDto.InvoiceHeader.InvoiceId}");
                    return BadRequest("Geçersiz belge.. Belgenizde eksiklik olabilir..");

                }

                var invoiceHeader = invoiceDto.InvoiceHeader;
                invoiceHeader.InvoiceLines = invoiceDto.InvoiceLines;


                await _invoiceRepository.AddAsync(invoiceHeader);
                await _invoiceRepository.SaveChangesAsync();

                myLog.Info($"{invoiceDto.InvoiceHeader.InvoiceId} idli fatura başarıyla kaydedildi.");
               return Created("", new { message = "Fatura başarıyla kaydedildi.", invoiceId = invoiceDto.InvoiceHeader.InvoiceId });
            }
            catch (System.Exception ex)
            {
                myLog.Error($"{invoiceDto.InvoiceHeader.InvoiceId} idli faturanın işlenmesi sırasında hata oluştu:", ex);
                return BadRequest($"{invoiceDto.InvoiceHeader.InvoiceId} idli faturanın işlenmesi sırasında hata oluştu: {ex.Message}");
            }

        }

        /// <summary>
        /// Tüm Faturaları Listeleyen method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            try
            {
                var invoices = await _invoiceRepository.GetAllInvoicesAsync();
                if (invoices == null)
                {
                    myLog.Error($"Herhangi bir fatura bulunamamıştır.");
                    return NotFound();
                }

                return Ok(invoices);
            }
            catch (System.Exception ex)
            {
                myLog.Error($"Faturaların Db'den alınması sırasında hata oluştu: ", ex);
                return BadRequest("Faturaların Db'den alınması sırasında hata oluştu");
            }

        }


        /// <summary>
        /// Bir Faturaya ait detay bilgileri döner
        /// </summary>
        /// <param name="id">Fatura başlık içinde bulunan fatura idsi </param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceLines(string id)
        {
            try
            {
                var invoiceLines = await _invoiceRepository.GetInvoiceLinesAsync(id);
                if (invoiceLines == null)
                {
                    myLog.Error($"{id} 'li Faturaya ait satır bulunamamıştır.");

                    return NotFound();
                }
               
                InvoiceDto dto=new InvoiceDto();
                dto.InvoiceHeader = invoiceLines;
                dto.InvoiceLines=invoiceLines.InvoiceLines;

                return Ok(dto);
            }
            catch (System.Exception ex)
            {

                myLog.Error($"{id}  Faturasına ait Detay bilgisi alınırken hata oluştu: ", ex);
                return BadRequest("{id}  Faturasına ait Detay bilgisi alınırken hata oluştu");
            }

        }
    }
}
