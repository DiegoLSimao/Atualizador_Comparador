using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Atualizador
{
    internal class CompararArquivos
    {
        // Método para comparar dois arquivos usando SHA256
        public bool ArquivoIgual(ZipArchiveEntry zipEntry, string existingFilePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Calcula o hash do arquivo existente
                using (FileStream fileStream = File.OpenRead(existingFilePath))
                {
                    byte[] existingFileHash = sha256.ComputeHash(fileStream);

                    // Calcula o hash do arquivo dentro do ZIP
                    using (Stream zipEntryStream = zipEntry.Open())
                    {
                        byte[] zipFileHash = sha256.ComputeHash(zipEntryStream);

                        // Compara os hashes
                        return StructuralComparisons.StructuralEqualityComparer.Equals(existingFileHash, zipFileHash);
                    }
                }
            }
        }
    }
}
