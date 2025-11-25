using System.Collections.Generic;
using AgendaTelefonicaApi.Models;

namespace AgendaTelefonicaApi.Repositories
{
    public interface IContatoRepository
    {
        IEnumerable<Contato> ObterTodos();
        Contato? ObterPorId(int id);
        Contato? ObterPorNome(string nome);
        Contato Criar(Contato contato);
        bool Atualizar(Contato existing, Contato newContato);
        bool Remover(int id);
        IEnumerable<Contato> BuscarPorNome(string nome);
    }
    
}
