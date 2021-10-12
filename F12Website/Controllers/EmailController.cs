using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using F12Website.Utils;
using Microsoft.Extensions.Configuration;
using F12Website.Models;
using System.Threading;

namespace F12Website.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly EmailHelper _emailHelper;

        public EmailController(IConfiguration config, EmailHelper emailHelper)
        {
            _config = config;
            _emailHelper = emailHelper;
        }




        [HttpPost]


        public async Task<ActionResult<string>> SendEmail(ContactFormModel contactFormModel)
        {
            try
            {
                var emailResult = await _emailHelper.SendEmailAsync(contactFormModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }










        }
    }
}
