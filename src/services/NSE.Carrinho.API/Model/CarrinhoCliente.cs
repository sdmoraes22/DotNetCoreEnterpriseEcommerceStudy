﻿using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Carrinho.API.Model
{
    public class CarrinhoCliente
    {
        public CarrinhoCliente(Guid clienteId)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
        }
        public CarrinhoCliente() { }
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public List<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();
        public ValidationResult ValidationResult { get; set; }

        internal void CalcularValorCarrinho()
        {
            ValorTotal = Itens.Sum(p => p.CalcularValor());
        }

        internal bool CarrinhoItemExistente(CarrinhoItem item)
        {
            return Itens.Any(p => p.ProdutoId == item.ProdutoId);
        }

        internal CarrinhoItem ObterProdutoPorId(Guid id)
        {
            return Itens.FirstOrDefault(p => p.ProdutoId == id);
        }

        internal void AdicionarItem(CarrinhoItem item)
        {
            item.AssociarCarrinho(Id);

            if (CarrinhoItemExistente(item)) 
            {
                var itemExistente = ObterProdutoPorId(item.ProdutoId);
                itemExistente.AdicionarUnidades(item.Quantidade);
                item = itemExistente;
                Itens.Remove(itemExistente);
            }

            Itens.Add(item);
            CalcularValorCarrinho();
        }

        internal void AtualizarItem(CarrinhoItem item)
        {
            item.AssociarCarrinho(Id);

            var itemExistente = ObterProdutoPorId(item.ProdutoId);
            
            Itens.Remove(itemExistente);
            Itens.Add(item);

            CalcularValorCarrinho();
        }

        internal void AtualizarUnidades(CarrinhoItem item, int unidades)
        {
            item.AtualizarUnidades(unidades);
            AtualizarItem(item);
        }

        internal void RemoverItem(CarrinhoItem item)
        {
            Itens.Remove(ObterProdutoPorId(item.ProdutoId));
            CalcularValorCarrinho();
        }

        internal bool EhValido()
        {
            var erros = Itens.SelectMany(i => new CarrinhoItem.ItemCarrinhoValidation().Validate(i).Errors).ToList();
            erros.AddRange(new CarrinhoClientValidation().Validate(this).Errors);
            ValidationResult = new ValidationResult(erros);
            
            return ValidationResult.IsValid;
        }

        public class CarrinhoClientValidation : AbstractValidator<CarrinhoCliente>
        {
            public CarrinhoClientValidation()
            {
                RuleFor(c => c.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Cliente não reconhecido");

                RuleFor(c => c.Itens.Count)
                    .GreaterThan(0)
                    .WithMessage("O carrinho não possui itens");

                RuleFor(c => c.ValorTotal)
                    .GreaterThan(0)
                    .WithMessage("O valor total do carrinho precisa ser maior que 0");
            }
        }
    }
}
