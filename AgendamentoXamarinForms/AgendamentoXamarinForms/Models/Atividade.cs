using System;
using System.Linq;

namespace AgendamentoXamarinForms.Models
{
    public class Atividade
    {
        public int? idAtividadeSessao { get; set; }
        public string nome { get; set; }
        public DateTime data { get; set; }
        public int capacidade { get; set; }
        public int ocupacao { get; set; }
        public string professor { get; set; }
        public string area { get; set; }
        public int status { get; set; }
        public int diaSemana { get; set; }
        public bool flNumerarVagas { get; set; }
        public long? horaTick { get; set; }
        public long? duracaoTick { get; set; }
        public string inicio { get; set; }
        public string fim { get; set; }
        public string descricao { get; set; }
        public string urlImagem { get; set; }
        public string[] niveis { get; set; }
        public Lugare[] lugares { get; set; }
        public string titulo { get; set; }
        public string[] mesagem { get; set; }

        //tela
        public string Horario => $"{inicio?.Substring(0, 5)} às {fim?.Substring(0, 5)}";
        public string Capacidade => $"{ocupacao}/{capacidade}";
        public string Color => Colors.Values[status];
        public Button Botao => ButtonsValue.Buttons[status];
        public string DiaSemana => DiaSemanaStr.Dia[diaSemana];
        public string Niveis => niveis == null || !niveis.Any() ? "Nenhum" : string.Join(", ", niveis);
    }

    public class Lugare
    {
        public int numero { get; set; }
        public bool disponivel { get; set; }
    }
    public class Button
    {
        public string Text { get; set; }
        public ButtonValue Value { get; set; }
        public bool Action { get; set; }
    }
    public static class ButtonsValue
    {
        public static Button[] Buttons = new Button[]
        {
            new Button { Text = "Livre", Value = ButtonValue.Livre, Action = false },
            new Button { Text = "Participar", Value = ButtonValue.Participar, Action = true },
            new Button { Text = "Entrar na fila", Value = ButtonValue.Fila, Action = true },
            new Button { Text = "Encerrada", Value = ButtonValue.Encerrada, Action = false },
            new Button { Text = "Restrita", Value = ButtonValue.Restrita, Action = false },
            new Button { Text = "Sair", Value = ButtonValue.Sair, Action = true }
        };
    }

    public static class DiaSemanaStr
    {
        public static string[] Dia = new string[]
        {
            "Dom","Seg","Ter", "Qua","Qui","Sex","Sab"
        };
    }

    public static class Colors
    {
        public static string[] Values = new string[]
        {
            "#fff",
            "#11a51b",
            "#ff0000",
            "#f6d510",
            "#f6d510",
            "#11a51b"
        };
    }

    public enum ButtonValue
    {
        Livre,
        Participar,
        Fila,
        Encerrada,
        Restrita,
        Sair
    }
}
