﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Sample.Kadastro.Infraestrutura.Comuns;
using Sample.Kadastro.Infraestrutura.Persistencia.Repositories;
using Sample.Kadastro.Infraestrutura.Persistencia.UnitOfWork;
using Sample.Kadastro.Dominio.Services;
using Sample.Kadastro.Aplicacao;
using Sample.Kadastro.ServicoDistribuido.Contracts;
using Sample.Kadastro.ServicoDistribuido.Extensions;

namespace Sample.Kadastro.ServicoDistribuido
{
    public class KadastroServiceHost : IKadastroServiceHost
    {
        #region Atributos

        private readonly IUsuarioAppService _usuarioAppService;
        private readonly ITarefaAppService _tarefaAppService;

        #endregion

        #region Construtor

        public KadastroServiceHost()
        {
            //context
            var unit = new MainUnitOfWork();

            //repositories
            var usuarioRepository = new UsuarioRepository(unit);
            var pontoRepository = new PontoRepository(unit);
            var intervaloRepository = new IntervaloRepository(unit);
            var tarefaRepository = new TarefaRepository(unit);

            //services
            var usuarioService = new UsuarioService(usuarioRepository);
            var tarefaService = new TarefaService(tarefaRepository);
            //var pontoRepository = new PontoService(pontoRepository, intervaloRepository);

            //applications
            _usuarioAppService = new UsuarioAppService(usuarioRepository, usuarioService);
            _tarefaAppService = new TarefaAppService(tarefaRepository, tarefaService);
        }

        #endregion

        #region Operações de Usuário

        public BusinessResponse<bool> Autenticar(string login, string senha)
        {
            return _usuarioAppService.Autenticar(login, senha);
        }

        public UsuarioDataContract ObterUsuario(string id)
        {
            return _usuarioAppService.Obter(int.Parse(id)).ToUsuarioDataContract();
        }

        public UsuarioDataContract ObterUsuarioPeloLogin(string login)
        {
            return _usuarioAppService.Obter(login).ToUsuarioDataContract();
        }

        public List<UsuarioDataContract> ListarUsuarios()
        {
            return _usuarioAppService.Obter().ToUsuarioDataContract();
        }

        public BusinessResponse<bool> SalvarUsuario(UsuarioDataContract usuario)
        {
            //UsuarioDataContract usuario = new UsuarioDataContract();

            //usuario.Login = login;
            //usuario.Senha = senha;
            //usuario.Email = email;
            //usuario.Status = status;

            //if (!string.IsNullOrEmpty(id))
                //usuario.Id = int.Parse(id);

            return _usuarioAppService.Salvar(usuario.ToUsuarioDTO());
        }

        public BusinessResponse<bool> ExcluirUsuario(int id)
        {
            return _usuarioAppService.Excluir(id);
        }

        #endregion

        #region Operações de Ponto e Intervalos

        #endregion

        #region Operações de Tarefas

        public List<TarefaDataContract> ListarTarefas(string login)
        {
            return _tarefaAppService.Obter(login).ToTarefaDataContract();
        }

        public BusinessResponse<bool> SalvarTarefa(TarefaDataContract tarefa)
        {
            return _tarefaAppService.Salvar(tarefa.ToTarefaDTO());
        }

        public BusinessResponse<bool> ExcluirTarefa(int id)
        {
            return _tarefaAppService.Excluir(id);
        }

        public BusinessResponse<bool> ExecutarTarefa(int id)
        {
            return _tarefaAppService.Executar(id);
        }

        #endregion

        #region Outras Operações

        public List<ItemListaDataContract> ListarPerfisDeAcesso()
        {
            return _usuarioAppService.ObterPerfilDeAcesso().ToPerfilAcessoDataContract();
        }

        #endregion
    }
}
