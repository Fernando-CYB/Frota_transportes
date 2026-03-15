using System;
class Program
{
    public interface IManutencao
    {
        decimal CalcularCustoRevisao();
    }

    public abstract class Veiculo : IManutencao
    {
        public string Placa { get; private set; }
        public decimal Tanque { get; protected set; }
        public Veiculo(string placa, decimal combustivelInicial)
        {
            if (combustivelInicial < 0)
                throw new ArgumentException("O combustível inicial não pode ser negativo.");
            Placa = placa;
            Tanque = combustivelInicial;
        }
        public abstract void Viajar(decimal distanciaKm);
        public abstract decimal CalcularPedagio();
        public abstract decimal CalcularCustoRevisao();
    }

    public class Carro : Veiculo
    {
        public Carro(string placa, decimal combustivelInicial) : base(placa, combustivelInicial)
        {
            
        }
        public override void Viajar(decimal distanciaKm)
        {
            decimal combustivelNecessario = distanciaKm / 10;
            if(Tanque - combustivelNecessario < 0) 
                throw new InvalidOperationException("Combustível insuficiente para a viagem.");
            Tanque -= combustivelNecessario;
        }
        public override decimal CalcularPedagio() => 8.50m; // Pedágio fixo para carros
        public override decimal CalcularCustoRevisao() => 300.00m; // Custo fixo de revisão para carros
    }

    public class Caminhao : Veiculo
    {
        public int QuantidadeEixos { get; private set; }
        public Caminhao(string placa, decimal combustivelInicial, int quantidadeEixos) : base(placa, combustivelInicial)
        {
            QuantidadeEixos = quantidadeEixos;
        }
        public override void Viajar(decimal distanciaKm)
        {
            decimal combustivelNecessario = distanciaKm / 4; // Consumo maior para caminhões
            if(Tanque - combustivelNecessario < 0) 
                throw new InvalidOperationException("Combustível insuficiente para a viagem.");
            Tanque -= combustivelNecessario;
        }
        public override decimal CalcularPedagio() => 8.50m * QuantidadeEixos; // Pedágio baseado no número de eixos
        public override decimal CalcularCustoRevisao() => 1500.00m; // Custo de revisão baseado no número de eixos
    }

    static void SimularViagemPolimorfica(Veiculo v, decimal quilometros)
    {
        try
        {
            Console.WriteLine($"\n--- A INICIAR VIAGEM COM O VEÍCULO: {v.Placa} ---");
            v.Viajar(quilometros);
            Console.WriteLine($"Sucesso! Restou no tanque: {v.Tanque:F2} Litros");
            Console.WriteLine($"Custo de Portagem (Pedágio): {v.CalcularPedagio():C}");
            Console.WriteLine($"Previsão de Revisão na Oficina:{ v.CalcularCustoRevisao():C}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ALERTA NA VIAGEM: {ex.Message}");
        }
    }
    static void Main()
    {
        Console.WriteLine("--- CADASTRO DA FROTA ---");
        // 1. Cadastrando o Carro
        Console.WriteLine("\n[A] Configurar o Carro do Gerente");
        Console.Write("Digite a Placa: ");
        string placaCarro = Console.ReadLine();
        Console.Write("Combustível Inicial (Litros): ");
        decimal tanqueCarro = decimal.Parse(Console.ReadLine());
        Carro carroGerente = null;
        try
        {
            carroGerente = new Carro(placaCarro, tanqueCarro);
            Console.WriteLine("Carro registado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO NO REGISTO: {ex.Message}");
        }
        // 2. Cadastrando o Caminhão
        Console.WriteLine("\n[B] Configurar o Caminhão de Carga");
        Console.Write("Digite a Placa: ");
        string placaCam = Console.ReadLine();
        Console.Write("Combustível Inicial (Litros): ");
        decimal tanqueCam = decimal.Parse(Console.ReadLine());
        Console.Write("Quantidade de Eixos: ");
        int eixosCam = int.Parse(Console.ReadLine());
        Caminhao caminhaoCarga = null;
        try
        {
            caminhaoCarga = new Caminhao(placaCam, tanqueCam, eixosCam);
            Console.WriteLine("Caminhão registado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO NO REGISTO: {ex.Message}");
        }
        // 3. A Simulação da Viagem
        Console.WriteLine("\n--- SIMULAÇÃO DE ROTA ---");
        Console.Write("Qual a distância do trajeto (Km)? ");
        decimal distancia = decimal.Parse(Console.ReadLine());
        // Enviamos os dois veículos diferentes para o mesmo método polimórfico
        if (carroGerente != null)
        {
            SimularViagemPolimorfica(carroGerente, distancia);
        }
        if (caminhaoCarga != null)
        {
            SimularViagemPolimorfica(caminhaoCarga, distancia);
        }
        Console.WriteLine("\nSimulação concluída. Pressione ENTER para sair.");
        Console.ReadLine();
    }
}