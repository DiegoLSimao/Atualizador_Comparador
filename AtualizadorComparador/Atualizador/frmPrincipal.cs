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


        public frmPrincipal()
        {
            InitializeComponent();
            CarregarVersao();
        }

        private void CarregarVersao()
        {
            var versao = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text += $" - Versão: {versao}";
        }

        public void EscreverStatus(string message)
        {
            txtStatus.AppendText(string.Concat(message, Environment.NewLine));
            txtStatus.ScrollToCaret();
        }

        private void tmrListaExecucao_Tick(object sender, EventArgs e)
        {
            try
            {
                ExecutarMaquinaEstado();
            }
            catch (Exception ex)
            {
                EscreverStatus($"Ocorreu uma exceção: {ex.Message}");
                tmrListaExecucao.Enabled = false;
            }
        }

        private void ExecutarMaquinaEstado()
        {
            switch (Lista_Execucao)
            {
                case 0:
                    EscreverStatus("Inicio !");
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
                    EscreverStatus($"Arquivos Idênticos: {Arquivos_Identicos}");
                    EscreverStatus($"Arquivos Novos: {Arquivos_Novos}");
                    Lista_Execucao++;
                    break;

                case 4:
                    tmrListaExecucao.Interval = 2000;
                    Lista_Execucao++;
                    break;

                case 5:
                    EscreverStatus("");
                    EscreverStatus("--- Click em qualquer area branca para cancelar o fechamento automático ---");
                    EscreverStatus("");
                    EscreverStatus("Irá fechar em 3");
                    Lista_Execucao++;
                    break;

                case 6:
                    EscreverStatus("Irá fechar em 2");
                    Lista_Execucao++;
                    break;

                case 7:
                    EscreverStatus("Irá fechar em 1");
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

            CompararArquivos compArquivo = new CompararArquivos();

            // Extrair apenas arquivos que são diferentes ou não existentes
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.Combine(extractPath, entry.FullName);

                    if (File.Exists(destinationPath))
                    {
                        // Comparar o arquivo existente com o do ZIP
                        if (!compArquivo.ArquivoIgual(entry, destinationPath))
                        {
                            // Substituir o arquivo se forem diferentes
                            entry.ExtractToFile(destinationPath, true);
                            EscreverStatus($"Arquivo sobrescrito: {destinationPath}");
                            Arquivos_Sobrescritos++;
                        }
                        else
                        {
                            EscreverStatus($"Arquivo idêntico, não sobrescrito: {destinationPath}");
                            Arquivos_Identicos++;
                        }
                    }
                    else
                    {
                        // Extrair se o arquivo não existir
                        entry.ExtractToFile(destinationPath);
                        EscreverStatus($"Novo arquivo extraído: {destinationPath}");
                        Arquivos_Novos++;
                    }
                    Thread.Sleep(20);
                }
            }
        }

        private void txtStatus_Click(object sender, EventArgs e)
        {
            EscreverStatus("Encerramento Automático Cancelado");
            tmrListaExecucao.Enabled = false;
        }
    }
}
