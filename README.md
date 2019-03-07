## Provedené optimalizace:
### Shlukování více kostek do jednoho meshe
Základní optimalizace, bez které nejde větší terény generovat. Každý blok 16x16x16 je tvořen jedním meshem, který obsahuje pouze strany kostek, které jsou viditelné.

### Postupné postupné generování terénu podle pohybu hráče
Vždy když se hráč posune o jeden blok, bloky nejdále od něj se přesunou na stranu na kterou se pohnul a přegenerují se. Jejich přegenerování je dále rozfázované, aby nedocházelo k zásekům hry, když se začne přegenerovávat 16 bloků najednou.

## Další možné optimalizace generování terénu:
### Threading
V tuhle chvíli používám nové vlákno pokaždé když generuji mesh pro jeden blok. Což je lepší než všechno generovat na hlavním vlákně, ale zdaleka ne ideální. 

Ideální by asi bylo změnit implementaci aby mohla pracovat pomocí Unity Jobs System, nebo vytvořit nějaký vlastní producent/consumer pattern, který by používal jenom únosný počet vláken a neničil by je pokaždé co vygeneruje jeden chunk.

### Generování perlin noise
Pokaždé když se generuje chunk, generuji pro něj noise. Ideální by bylo, aby noise byl předpočítaný a ve chvíli generování meshe už se pouze braly hodnoty.
