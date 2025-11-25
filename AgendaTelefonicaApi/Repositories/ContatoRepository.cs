using System.Collections.Generic;
using System.Linq;
using AgendaTelefonicaApi.Models;

namespace AgendaTelefonicaApi.Repositories
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly AgendaDbContext dbContext;
        public ContatoRepository(AgendaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Contato> ObterTodos()
        {
            return dbContext.Contatos;
        }

        public Contato? ObterPorId(int id)
        {
            return dbContext.Contatos.Find(id);
        }

        public Contato? ObterPorNome(string nome)
        {
            return dbContext.Contatos.FirstOrDefault(c => c.Nome.ToLower() == nome.ToLower());
        }

        public Contato Criar(Contato contato)
        {
            dbContext.Contatos.Add(contato);
            dbContext.SaveChanges();

            return contato;
        }

        public bool Atualizar(Contato existingContato, Contato newContato)
        {
            existingContato.Nome = newContato.Nome;
            existingContato.Telefone = newContato.Telefone;

            dbContext.Contatos.Update(existingContato);
            dbContext.SaveChanges();

            return true;
        }

        public bool Remover(int id)
        {
            var contato = dbContext.Contatos.Find(id);
            if (contato != null)
            {
                dbContext.Contatos.Remove(contato);
                dbContext.SaveChanges();
            }

            return true;
        }

        public IEnumerable<Contato> BuscarPorNome(string nome)
        {
            return dbContext.Contatos
                .Where(c => c.Nome.ToLower().Contains(nome.ToLower()));
        }
    }
}
