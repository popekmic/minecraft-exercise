## Možné optimalizace generování terénu:
### 1.Threading
V tuhle chvíli používám nové vlákno pokaždé když generuji mesh pro jeden chunk 16x16. Což je lepší než všechno generovat na hlavním vlákně, ale zdaleka ne ideální. 

Ideální by asi bylo změnit implementaci aby mohla pracovat pomocí Unity Jobs System, nebo vytvořit nějaký vlastní producent/consumer pattern, který by používal jenom únosný počet vláken a neničil by je pokaždé co vygeneruje jeden chunk.
