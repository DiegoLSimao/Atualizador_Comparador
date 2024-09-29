using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Atualizador
{
    public partial class frmPrincipal : Form
    {
        private int Lista_Execucao { get; set; }
        private int Arquivos_Sobrescritos { get; set; }
        private int Arquivos_Identicos { get; set; }
        private int Arquivos_Novos { get; set; }
        private int Pastas_Criadas { get; set; }
        private int Pastas_Identicas { get; set; }
        private int Numero_Linha { get; set; }
        private bool Cancelar_Fechamento {  get; set; }


        public frmPrincipal()
        {
            InitializeComponent();
            CarregarVersao();
        }

        private void CarregarVersao()
        {
            var versao = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text += $" - Vers�o: {versao}";
        }

        public void EscreverStatus(string message)
        {
            
            txtStatus.AppendText(string.Concat($"{Numero_Linha}: ",message, Environment.NewLine));
            txtStatus.ScrollToCaret();
            Numero_Linha++;
            Thread.Sleep(25);
        }

        private void tmrListaExecucao_Tick(object sender, EventArgs e)
        {
            try
            {
                ExecutarMaquinaEstado();
            }
            catch (Exception ex)
            {
                EscreverStatus($"Ocorreu uma exce��o: {ex.Message}");
                tmrListaExecucao.Enabled = false;
            }
        }

        private void ExecutarMaquinaEstado()
        {
            switch (Lista_Execucao)
            {
                case 0:
                    EscreverStatus("Inicio !");
                    Cancelar_Fechamento = true;
                    Lista_Execucao++;
                    break;

                case 1:
                    EscreverStatus("Leitura do arquivo Compactado");
                    CompararDescompactar();
                    Lista_Execucao++;
                    break;

                case 2:
                    EscreverStatus("Finalizado !");
                    Lista_Execucao++;
                    break;

                case 3:
                    EscreverStatus("--- Resumo ---");
                    EscreverStatus($"Arquivos Sobrescritos: {Arquivos_Sobrescritos}");
                    EscreverStatus($"Arquivos Id�nticos: {Arquivos_Identicos}");
                    EscreverStatus($"Arquivos Novos: {Arquivos_Novos}");
                    EscreverStatus($"Pastas Criadas: {Pastas_Criadas}");
                    EscreverStatus($"Pastas Identicas: {Pastas_Identicas}");
                    Lista_Execucao++;
                    break;

                case 4:

                    tmrListaExecucao.Interval = 2000;
                    Lista_Execucao++;
                    break;

                case 5:
                    EscreverStatus("");
                    EscreverStatus("--- Click em qualquer area branca para cancelar o fechamento autom�tico ---");
                    EscreverStatus("");
                    EscreverStatus("Ir� fechar em 3");
                    Cancelar_Fechamento = false;
                    Lista_Execucao++;
                    break;

                case 6:
                    EscreverStatus("Ir� fechar em 2");
                    Lista_Execucao++;
                    break;

                case 7:
                    EscreverStatus("Ir� fechar em 1");
                    Lista_Execucao++;
                    break;

                case 8:
                    Environment.Exit(0);
                    break;

                default:
                    break;
            }
 
        }




        private void CompararDescompactar()
        {
            string zipPath = @"C:\Users\Diego\Desktop\Teste\Compactado\NF_EMITIDAS.zip";
            string extractPath = @"C:\Users\Diego\Desktop\Teste\Descompactar";

            EscreverStatus($"Arquivo Compactado: {zipPath}");
            EscreverStatus($"Pasta para Descompactar: {extractPath}");
            EscreverStatus("- - - - - - - - - - - - - - - - - - - - -");

            CompararArquivos comparar = new CompararArquivos();

            // Extrair apenas arquivos que s�o diferentes ou n�o existentes
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.Combine(extractPath, entry.FullName);

                    // Verifica se a entrada � um diret�rio
                    if (entry.FullName.EndsWith("/"))
                    {
                        // Cria a pasta, se necess�rio
                        if (!Directory.Exists(destinationPath))
                        {
                            Directory.CreateDirectory(destinationPath);
                            EscreverStatus($"Pasta criada: {destinationPath}");
                            Pastas_Criadas++;
                        }
                        else
                        {
                            EscreverStatus($"Pasta j� existente: {destinationPath}");
                            Pastas_Identicas++;
                        }
                        continue; // Continuar pois n�o existe extra��o de pasta
                    }

                    // Verifica se entrada � um arquivo
                    if (File.Exists(destinationPath))
                    {
                        // Comparar o arquivo existente com o do ZIP
                        if (!comparar.ArquivoIgual(entry, destinationPath))
                        {
                            // Substituir o arquivo se forem diferentes
                            entry.ExtractToFile(destinationPath, true);
                            EscreverStatus($"Arquivo sobrescrito: {destinationPath}");
                            Arquivos_Sobrescritos++;
                        }
                        else
                        {
                            EscreverStatus($"Arquivo id�ntico, n�o sobrescrito: {destinationPath}");
                            Arquivos_Identicos++;
                        }
                    }
                    else
                    {
                        // Extrair se o arquivo n�o existir
                        entry.ExtractToFile(destinationPath);
                        EscreverStatus($"Novo arquivo extra�do: {destinationPath}");
                        Arquivos_Novos++;
                    }
                    
                }
            }
        }

        private void txtStatus_Click(object sender, EventArgs e)
        {
            if(!Cancelar_Fechamento)
            {
                EscreverStatus("Encerramento Autom�tico Cancelado");
                tmrListaExecucao.Enabled = false;
                Cancelar_Fechamento = true;
            }
            
        }
    }
}
