﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TesteMazzaFC.WebApi.Models;

namespace TesteMazzaFC.WebApi.Controllers
{
    public class ProdutoController : Controller
    {

        const string URL_BASE = "https://localhost:44306/api/";

        // GET: Produtos Api
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IEnumerable<ProdutoViewModel> produtos = null;

            using (var prod = new HttpClient())
            {
                prod.BaseAddress = new Uri(URL_BASE);

                var result = await prod.GetAsync("Produtos");

                if (result.IsSuccessStatusCode)
                {
                    produtos = await result.Content.ReadAsAsync<IList<ProdutoViewModel>>();
                }
                else
                {
                    produtos = Enumerable.Empty<ProdutoViewModel>();
                    ModelState.AddModelError(string.Empty, "Erro no servidor. Contate o Administrador.");
                }
            }

            return View(produtos);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            await CreateCategoriaViewBag();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProdutoViewModel produto)
        {
            if (produto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                using (var prod = new HttpClient())
                {
                    prod.BaseAddress = new Uri(URL_BASE);

                    var result = await prod.PostAsJsonAsync("Produtos", produto);

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    var error = await result.Content.ReadAsAsync<ErroViewModel>();

                    ModelState.AddModelError(string.Empty, error.Errors.FirstOrDefault());
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro no Servidor. Contacte o Administrador.");
            }
            
            await CreateCategoriaViewBag();
            return View(produto);
        }
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProdutoViewModel produto = null;
            using (var prod = new HttpClient())
            {
                prod.BaseAddress = new Uri($"{URL_BASE}Produtos/{id}");

                var responseTask = await prod.GetAsync("");

                if (responseTask.IsSuccessStatusCode)
                {
                    produto = await responseTask.Content.ReadAsAsync<ProdutoViewModel>();
                    await CreateCategoriaViewBag();
                }
            }
            return View(produto);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ProdutoViewModel produto)
        {
            if (produto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var prod = new HttpClient())
            {
                prod.BaseAddress = new Uri(URL_BASE);
  
                var result = await prod.PutAsJsonAsync<ProdutoViewModel>($"Produtos/{produto.Id}", produto);

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            await CreateCategoriaViewBag();
            return View(produto);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var prod = new HttpClient())
            {
                prod.BaseAddress = new Uri($"{URL_BASE}Produtos/{id}");

                var responseTask = await prod.DeleteAsync("");

                if (responseTask.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
             throw new HttpException();
        }

        private async Task CreateCategoriaViewBag()
        {
            IEnumerable<CategoriaViewModel> categorias = null;

            using (var catego = new HttpClient())
            {
                catego.BaseAddress = new Uri(URL_BASE);

                var result = await catego.GetAsync("Categoria");

                if (result.IsSuccessStatusCode)
                {
                    categorias = await result.Content.ReadAsAsync<IList<CategoriaViewModel>>();
                    ViewBag.categorias = categorias;
                }
            }
        }
    }
}