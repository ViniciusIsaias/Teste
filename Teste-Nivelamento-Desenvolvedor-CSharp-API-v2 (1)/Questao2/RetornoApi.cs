namespace Questao2
{
    public class RetornoApi
    {
        public int Page { get; set; }

        public int Per_Page { get; set; }

        public int Total { get; set; }

        public int Total_Pages { get; set; }

        public List<Jogos> Data { get; set; } = new List<Jogos>();
    }
}
