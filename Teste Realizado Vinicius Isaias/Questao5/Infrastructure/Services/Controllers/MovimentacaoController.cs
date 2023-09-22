using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Questao5.Domain.Entities;
using Questao5.Domain.ViewModel;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacaoController : ControllerBase
    {
        private static readonly string[] tipoMovimento = new[]
        {
            "C", "D"
        };

        private readonly IDatabaseContaCorrente _databaseContaCorrente;
        private readonly IDatabaseIdempotencia _databaseIdempotencia;
        private readonly IMapper _mapper;

        public MovimentacaoController(IDatabaseContaCorrente databaseContaCorrente, IMapper mapper, IDatabaseIdempotencia databaseIdempotencia)
        {
            _databaseContaCorrente = databaseContaCorrente;
            _mapper = mapper;
            _databaseIdempotencia = databaseIdempotencia;
        }

        [HttpPost]
        [Route("MovimentacaoContaCorrente")]
        public IActionResult MovimentacaoContaCorrente([FromBody] MovimentacaoContaDto movimentacaoContaViewModel)
        {
            var idIdempotencia = 0;

            try
            {
                if (!_databaseIdempotencia.ValidaIdempotencia(movimentacaoContaViewModel.IdRequisicao))
                {
                    idIdempotencia = _databaseIdempotencia.InsereIdempotencia(movimentacaoContaViewModel.IdRequisicao);

                    var conta = _mapper.Map<ContaCorrente>(movimentacaoContaViewModel);

                    var contaCorrente = _databaseContaCorrente.RetornaContaCorrente(conta.IdContaCorrente);

                    if (movimentacaoContaViewModel.Valor <= 0)
                    {
                        _databaseIdempotencia.UpdateIdempotencia(idIdempotencia, this.BadRequest().StatusCode.ToString() + ": INVALID_VALUE.");
                        return this.BadRequest("INVALID_VALUE.");
                    }

                    if (!tipoMovimento.Contains(conta.TipoMovimento.ToString()))
                    {
                        _databaseIdempotencia.UpdateIdempotencia(idIdempotencia, this.BadRequest().StatusCode.ToString() + ": INVALID_TYPE.");
                        return this.BadRequest("INVALID_TYPE.");
                    }

                    var idMovimento = _databaseContaCorrente.InserirMovimentacao(conta);

                    _databaseIdempotencia.UpdateIdempotencia(idIdempotencia, this.Ok().StatusCode.ToString() + $": Id movimento {idMovimento}.");
                    return this.Ok(idMovimento);
                }
                else
                {
                    var idempotencia = _databaseIdempotencia.RetornaIdempotencia(movimentacaoContaViewModel.IdRequisicao);
                    return this.BadRequest(idempotencia.Resultado);
                }
            }
            catch (ArgumentException ae)
            {
                _databaseIdempotencia.UpdateIdempotencia(idIdempotencia, this.BadRequest().StatusCode.ToString() + ": " + ae.Message);
                return this.BadRequest(ae.Message);
            }
            catch (Exception ex)
            {
                _databaseIdempotencia.UpdateIdempotencia(idIdempotencia, this.StatusCode(500).ToString() + ": " + ex.Message);
                return this.StatusCode(500);
            }
        }

        [HttpGet]
        [Route("Saldo/{idContaCorrente}")]
        public IActionResult Saldo(string idContaCorrente)
        {
            try
            {
                var contaCorrente = _databaseContaCorrente.RetornaContaCorrente(idContaCorrente);

                var saldo = _mapper.Map<SaldoDto>(_databaseContaCorrente.RetornaSaldo(idContaCorrente));

                return this.Ok(saldo);
            }
            catch (ArgumentException ae)
            {
                return this.BadRequest(ae.Message);
            }
            catch (Exception)
            {
                return this.StatusCode(500);
            }
        }
    }
}
