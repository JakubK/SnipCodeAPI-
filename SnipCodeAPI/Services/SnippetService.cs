﻿using SnipCodeAPI.Models;
using SnipCodeAPI.Repositories.Interfaces;
using SnipCodeAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SnipCodeAPI.Services
{
    public class SnippetService : ISnippetService
    {
        private readonly ISnippetRepository _snippetRepository;
        private readonly ISnippetFileRepository _snippetFileRepository;
        private readonly IDateTime _dateTime;

        public SnippetService(IDateTime dateTime, ISnippetRepository snippetRepository,
            ISnippetFileRepository snippetFileRepository)
        {
            _dateTime = dateTime;
            _snippetRepository = snippetRepository;
            _snippetFileRepository = snippetFileRepository;
        }

        public void Create(Snippet snippet)
        {
            foreach (var file in snippet.Files)
            {
                _snippetFileRepository.InsertSnippetFile(file);
            }
            snippet.Hash = GenerateHash(snippet.Name);
            snippet.CreationTime = _dateTime.Now.ToString("g");
            snippet.ExpirationTime = _dateTime.Now.AddMinutes(10).ToString("g");
            _snippetRepository.InsertSnippet(snippet);
        }

        public List<Snippet> GetSnippets() => _snippetRepository.GetSnippets().ToList();

        public Snippet GetSnippetById(int id, out Snippet snippet) => snippet = _snippetRepository.GetSnippetById(id);

        public bool DeleteSnippet(int id)
        {
            var snippet = _snippetRepository.GetSnippetById(id);
            if (snippet == null)
                return false;
            foreach (var file in snippet.Files)
            {
                _snippetFileRepository.DeleteSnippetFile(file.Id);
            }
            _snippetRepository.DeleteSnippet(id);
            return true;
        }

        public bool UpdateSnippet(int id, Snippet snippet) =>
            _snippetRepository.GetSnippetById(id) != null && _snippetRepository.UpdateSnippet(snippet);

        private static string GenerateHash(string text)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(text);
            var hash = md5.ComputeHash(inputBytes);
            var stringBuilder = new StringBuilder();
            foreach (var value in hash)
            {
                stringBuilder.Append(value.ToString("X2"));
            }

            return stringBuilder.ToString();
        }
    }
}
