/* DB MS SQL SERVER */

/* TASK 1 */
SELECT [NAME] 
FROM CITY 
WHERE COUNTRYCODE = 'JPN'

/* TASK 2 */
SELECT DISTINCT(CITY) 
FROM STATION 
WHERE ([ID]% 2) = 0 
ORDER BY CITY ASC

/* TASK 3 */
SELECT AVG(POPULATION) 
FROM CITY

/* TASK 4 */

-- V1 --
SELECT CAST(LONG_W AS DECIMAL(10,4))
FROM STATION
WHERE LAT_N = (SELECT MIN(LAT_N) FROM STATION WHERE LAT_N > 38.7780)

-- V2 --
SELECT TOP 1 CAST(ROUND(LONG_W, 4) AS DECIMAL(10, 4)) 
FROM STATION 
WHERE LAT_N > 38.7780 ORDER BY LAT_N

/* TASK 5 */
SELECT SUM(CITY.POPULATION)
FROM CITY
JOIN COUNTRY ON CITY.COUNTRYCODE = COUNTRY.CODE
WHERE COUNTRY.CONTINENT = 'Asia'

/* TASK 6 */
SELECT CITY.NAME AS NAME
FROM CITY
JOIN COUNTRY ON CITY.COUNTRYCODE = COUNTRY.CODE
WHERE COUNTRY.CONTINENT = 'Africa'