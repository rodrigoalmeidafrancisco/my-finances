namespace Domain.Queries
{
    public static class QueryTeste
    {


    }
}

/*
 
  public static class QueryAplicativoBase
    {
        public static Expression<Func<AplicativoBase, object>>[] Includes(bool todos = true, bool escopo = false)
        {
            List<Expression<Func<AplicativoBase, object>>> listaRetorno = [];

            if (todos)
            {
                listaRetorno.Add(x => x.ListaEscopo);
            }
            else
            {
                if (escopo)
                {
                    listaRetorno.Add(x => x.ListaEscopo);
                }
            }

            return listaRetorno.ToArray();
        }

        public static Expression<Func<AplicativoBase, object>> ColunaOrderByDataTable(string colunaOrdenacaoNome)
        {
            return colunaOrdenacaoNome switch
            {
                "Id" => x => x.Id,
                "UsuarioLog" => x => x.UsuarioLog,
                "DataAlteracao" => x => x.DataAlteracao,
                "Descricao" => x => x.Descricao,
                "Login" => x => x.Login,
                _ => x => x.Nome,
            };
        }

        public static Expression<Func<AplicativoBase, bool>> PorId(string id)
        {
            return x => x.Id == id;
        }

        public static Expression<Func<AplicativoBase, bool>> PorLoginPassword(string login, string password)
        {
            return x => x.Login == login && x.Password == password;
        }

        public static Expression<Func<AplicativoBase, bool>> PorCommand(CommandObterAcessoInterno command)
        {
            return x => x.Id == command.Identificador && x.Login == command.Login && x.Password == UsefulExtension.Criptografar(command.Password);
        }

        public static Expression<Func<AplicativoBase, bool>> PorCommand(CommandAplicativoObter command)
        {
            Expression<Func<AplicativoBase, bool>> expression = x => true;

            if (command.Id.EhNuloOuVazioOuEspacoEmBranco() == false)
            {
                expression = expression.AddExpression(x => x.Id == command.Id);
            }

            if (command.Nome.EhNuloOuVazioOuEspacoEmBranco() == false)
            {
                expression = expression.AddExpression(x => x.Nome == command.Nome.Trim());
            }

            return expression;
        }

        public static Expression<Func<AplicativoBase, bool>> PorCommand(CommandAplicativoSelecionar command)
        {
            Expression<Func<AplicativoBase, bool>> expression = x => true;

            if (command.Identificador.EhNuloOuVazioOuEspacoEmBranco() == false)
            {
                expression = expression.AddExpression(x => x.Id == command.Identificador);
            }

            if (command.Nome.EhNuloOuVazioOuEspacoEmBranco() == false)
            {
                expression = expression.AddExpression(x => x.Descricao.Contains(command.Nome));
            }

            if (command.ListaEscopo != null && command.ListaEscopo.Any())
            {
                expression = expression.AddExpression(x => x.ListaEscopo.Any(y => command.ListaEscopo.Contains(y.Descricao)));
            }

            if (command.ValorPesquisa.EhNuloOuVazioOuEspacoEmBranco() == false)
            {
                expression = expression.AddExpression(x =>
                    x.Id.ToLower().Contains(command.ValorPesquisa.ToLower()) ||
                    x.Nome.ToLower().Contains(command.ValorPesquisa.ToLower()) ||
                    x.Descricao.ToLower().Contains(command.ValorPesquisa.ToLower())
                );
            }

            return expression;
        }
    }

 
 */