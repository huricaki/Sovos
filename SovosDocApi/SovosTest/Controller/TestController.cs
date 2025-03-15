using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SovosDocApi.Controllers;
using SovosDocApi.Models;
using SovosDocApi.Repositories.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SovosTest.Controller
{
    public class TestController
    {
        private readonly Mock<IInvoiceRepository> _mockRepo;
        private readonly InvoiceController _controller;

        public TestController()
        {
            _mockRepo = new Mock<IInvoiceRepository>();
            _controller = new InvoiceController(_mockRepo.Object);
        }

        
        [Fact]
        public async Task GetInvoices_ShouldReturnOk_WhenInvoiceExists()
        {

            var mockInvoice = new List<InvoiceHeader>{
                new InvoiceHeader {InvoiceId="SVS202300000001"},
                new InvoiceHeader {InvoiceId="SVS202300000002"},
                new InvoiceHeader {InvoiceId="SVS202300000003"},
                new InvoiceHeader {InvoiceId="SVS202300000004"},
                new InvoiceHeader {InvoiceId="SVS202300000005"},

            };
           var mockInvoices= _mockRepo.Setup(rep => rep.GetAllInvoicesAsync()).ReturnsAsync(mockInvoice);

            var invoices = await _controller.GetInvoices();

            var okResult = Assert.IsType<OkObjectResult>(invoices); // dönen cevabın 200 ok olup olmadığını kontrol eder
            var returnedInvoice = Assert.IsType<List<InvoiceHeader>>(okResult.Value); // ok dönen methodun gerçekten invoiceHeader türünde olup olmadığını kontrol eder


            Assert.Equal(5, returnedInvoice.Count);


        }
        [Fact]
        public async Task CreateInvoice_ShouldReturnCreated_WhenInvoiceIsValid()
        {
            var newInvoice = new InvoiceDto
            {
                InvoiceHeader = new InvoiceHeader
                {
                    InvoiceId = "SVS202300000011",
                    SenderTitle = "HC Firma AŞ",
                    ReceiverTitle = "CA Firma AŞ",
                    Date = DateOnly.Parse("2023-01-05"),
                    Email = "hurcak04@gmail.com"
                },

                InvoiceLines = new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        LineId = 1,
                        Name = "gluten2",
                        Quantity = 5,
                        UnitCode = "Adet",
                        UnitPrice = 10
                    }
                }
            };

            var invoiceHeader = newInvoice.InvoiceHeader;
            invoiceHeader.InvoiceLines = newInvoice.InvoiceLines;

            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<InvoiceHeader>()))
              .Returns(Task.CompletedTask); // Eğer metot void/Task döndürüyorsa


            var result = await _controller.CreateInvoice(newInvoice);

            var createdResult = Assert.IsType<CreatedResult>(result);

            Assert.Equal("201", createdResult.StatusCode.ToString());
        }
        [Fact]
        public async Task GetInvoiceLines_ShouldReturnOk_WhenInvoiceExists()
        {
            string invoiceId = "SVS202300000001";

            var fakeInvoiceHeader = new InvoiceHeader
            {
                InvoiceId = invoiceId,
                SenderTitle = "Gönderici Firma",
                ReceiverTitle = "Alıcı Firma",
                Date = DateOnly.Parse("2023-01-05"),
                Email = "test@example.com",
                InvoiceLines = new List<InvoiceLine>
                {
                    new InvoiceLine { Id = 1, Name = "1.Ürün" },
                    new InvoiceLine { Id = 2, Name = "2.Ürün" },
                    new InvoiceLine { Id = 3, Name = "3.Ürün" }
                }
            };

           var mockInvoice= _mockRepo.Setup(repo => repo.GetInvoiceLinesAsync(invoiceId))
                     .ReturnsAsync(fakeInvoiceHeader);

            
            var result = await _controller.GetInvoiceLines(invoiceId);

            var okResult = Assert.IsType<OkObjectResult>(result); // 200 OK olup olmadığını kontrol et
            var responseHeader = Assert.IsType<InvoiceDto>(okResult.Value); // Dönüş tipinin InvoiceHeader olup olmadığını kontrol et
            var returnedLines = responseHeader.InvoiceLines; // InvoiceLines listesini al

            Assert.NotNull(returnedLines); // Null değilse
            Assert.Equal(3, returnedLines.Count); //Line Countları eşit mi
            Assert.Equal("1.Ürün", returnedLines[0].Name); //ürün adları uyuşuyor mu
            Assert.Equal("2.Ürün", returnedLines[1].Name);
            Assert.Equal("3.Ürün", returnedLines[2].Name);
        }

        [Fact]
        public async Task GetInvoiceLines_ShouldReturnNotFound_WhenInvoiceDoesNotExist()
        {
            string invoiceId = "999"; // Geçersiz InvoiceId
            _mockRepo.Setup(repo => repo.GetInvoiceLinesAsync(invoiceId))
                     .ReturnsAsync((InvoiceHeader)null); // Boş değer döner

            var result = await _controller.GetInvoiceLines(invoiceId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
