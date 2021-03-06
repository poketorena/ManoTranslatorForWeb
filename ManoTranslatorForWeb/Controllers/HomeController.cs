﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ManoTranslatorForWeb.Models;
using ManoTranslatorCLI;
using ManoTranslatorForWeb.Models;

namespace ManoTranslatorForWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(ManoText manoText)
        {
            return View(manoText);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Convert(string id, [Bind("Id,InputText,OutputText")] ManoText manoText)
        {
            var inputText = string.Empty;
            var outputText = string.Empty;
            if (id != manoText.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                inputText = manoText.InputText;
                var translator = new Translator();
                outputText = translator.Encode(manoText.InputText ?? string.Empty);
            }

            return RedirectToAction(nameof(Index), new ManoText { Id = Guid.NewGuid().ToString(), InputText = inputText, OutputText = outputText });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deconvert(string id, [Bind("Id,InputText,OutputText")] ManoText manoText)
        {
            var inputText = string.Empty;
            var outputText = string.Empty;
            if (id != manoText.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                inputText = manoText.InputText;
                var translator = new Translator();
                outputText = translator.Decode(manoText.InputText ?? string.Empty);
            }

            return RedirectToAction(nameof(Index), new ManoText { Id = Guid.NewGuid().ToString(), InputText = inputText, OutputText = outputText });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
