--Você precisa escrever um comando select que retorne o assunto, o ano e a quantidade de ocorrências, 
--filtrando apenas assuntos que tenham mais de 3 ocorrências no mesmo ano.
--O comando deve ordenar os registros por ANO e por QUANTIDADE de ocorrências de forma DECRESCENTE.


select assunto, ano, count(id) as qtdOcorrencia from atendimentos
group by assunto, ano
having count(id) > 3
order by ano desc, qtdOcorrencia desc