using AgendamentoXamarinForms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AgendamentoXamarinForms.Services
{
    public class EvoApi
    {
        private readonly string _baseUrl = "https://w12evo.com/EvoApi/api/v1/";
        private HttpClient client = new HttpClient();
        public EvoApi(string token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<Tuple<Errors, List<Atividade>>> ObterTodasAtividades(string clienteToken, DateTime data, bool apenasDisponiveis)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{_baseUrl}atividade?token={clienteToken}&data={data.ToString("yyyy-MM-dd")}&flApenasDisponiveis={apenasDisponiveis}&mobile=true&wod=false");
                var result = await response.Content.ReadAsStringAsync();
                if (result == null || (response.StatusCode != System.Net.HttpStatusCode.BadRequest && response.StatusCode != System.Net.HttpStatusCode.OK))
                    return null;
                if (result.Contains("errors"))
                    return new Tuple<Errors, List<Atividade>>(JsonConvert.DeserializeObject<Errors>(result), null);
                else
                    return new Tuple<Errors, List<Atividade>>(null, JsonConvert.DeserializeObject<List<Atividade>>(result));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Tuple<UsuarioError, Usuario>> Logar(string usuario, string senha)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://w12evo.com.br/Mobile/API/Login?Username={usuario}&Password={senha}");
                var result = await response.Content.ReadAsStringAsync();
                if (result == null || (response.StatusCode != System.Net.HttpStatusCode.BadRequest && response.StatusCode != System.Net.HttpStatusCode.OK))
                    return null;
                if (!result.Contains("Success"))
                    return new Tuple<UsuarioError, Usuario>(JsonConvert.DeserializeObject<UsuarioError>(result), null);
                else
                    return new Tuple<UsuarioError, Usuario>(null, JsonConvert.DeserializeObject<Usuario>(result));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Tuple<Errors, string>> ParticiparDaAtividade(string clienteToken, int idConfiguracao, DateTime data, int? numeroVaga)
        {
            try
            {
                var url = $"{_baseUrl}atividade/participar?token={clienteToken}&idAtividadeSessao={idConfiguracao}&data={data.ToString("yyyy-MM-dd")}";
                url += numeroVaga != null ? ("&numeroDaVaga=" + numeroVaga) : "";
                HttpResponseMessage response =
                    await client.PostAsync(url, null);
                var result = await response.Content.ReadAsStringAsync();
                if (result == null || (response.StatusCode != System.Net.HttpStatusCode.BadRequest && response.StatusCode != System.Net.HttpStatusCode.OK))
                    return null;
                if (result.Contains("errors"))
                    return new Tuple<Errors, string>(JsonConvert.DeserializeObject<Errors>(result), null);
                else
                    return new Tuple<Errors, string>(null, result);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Tuple<Errors, string>> SairDaAtividade(string clienteToken, DateTime data, int idConfiguracao)
        {
            try
            {
                var url = $"{_baseUrl}atividade/sair?token={clienteToken}&idAtividadeSessao={idConfiguracao}&data={data.ToString("yyyy-MM-dd")}";
                HttpResponseMessage response =
                    await client.PostAsync(url, null);
                var result = await response.Content.ReadAsStringAsync();
                if (result == null || (response.StatusCode != System.Net.HttpStatusCode.BadRequest && response.StatusCode != System.Net.HttpStatusCode.OK))
                    return null;
                if (result.Contains("errors"))
                    return new Tuple<Errors, string>(JsonConvert.DeserializeObject<Errors>(result), null);
                else
                    return new Tuple<Errors, string>(null, result);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Tuple<Errors, string>> EntrarNaFilaDaAtividade(string clienteToken, DateTime data, int idConfiguracao)
        {
            try
            {
                var url = $"{_baseUrl}atividade/EntrarFila?token={clienteToken}&idAtividadeSessao={idConfiguracao}&data={data.ToString("yyyy-MM-dd")}";
                HttpResponseMessage response =
                    await client.PostAsync(url, null);
                var result = await response.Content.ReadAsStringAsync();
                if (result == null || (response.StatusCode != System.Net.HttpStatusCode.BadRequest && response.StatusCode != System.Net.HttpStatusCode.OK))
                    return null;
                if (result.Contains("errors"))
                    return new Tuple<Errors, string>(JsonConvert.DeserializeObject<Errors>(result), null);
                else
                    return new Tuple<Errors, string>(null, result);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
