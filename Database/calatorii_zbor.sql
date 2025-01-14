-- MySQL dump 10.13  Distrib 8.0.20, for Win64 (x86_64)
--
-- Host: localhost    Database: calatorii
-- ------------------------------------------------------
-- Server version	8.0.20

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `zbor`
--

DROP TABLE IF EXISTS `zbor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `zbor` (
  `IdZbor` int NOT NULL AUTO_INCREMENT,
  `NumeCompanie` longtext,
  `Destinatie` longtext,
  `DataPlecare` datetime(6) DEFAULT NULL,
  `GreutateMaximaBagaj` decimal(5,2) NOT NULL,
  `LocuriDisponibile` int NOT NULL,
  `Status` longtext,
  `Imbarcare` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Pret` decimal(10,2) NOT NULL DEFAULT '0.00',
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `TaxaSuplimentara` decimal(18,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`IdZbor`),
  UNIQUE KEY `IdZbor_UNIQUE` (`IdZbor`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `zbor`
--

LOCK TABLES `zbor` WRITE;
/*!40000 ALTER TABLE `zbor` DISABLE KEYS */;
INSERT INTO `zbor` VALUES (1,'Wizz Air','Roma','2024-12-12 14:30:00.000000',120.00,80,'Planificat','Suceava',25.00,0,10.00),(2,'Ryanair ','Paris','2024-10-10 14:00:00.000000',60.00,120,'Anulat','Roma',100.00,0,20.00),(3,'Wizz Air','Paris','2024-12-21 10:00:00.000000',20.00,3,'Planificat','Suceava',40.00,0,5.00),(4,'Blue Air','Suceava','2025-01-10 21:00:00.000000',40.00,7,'Planificat','Roma',60.00,0,30.00),(5,'Vueling ','Luxemburg','2025-01-12 22:00:00.000000',80.00,10,'Planificat','Londra',120.00,0,15.00),(6,'Wizz Air','Londra','2024-12-01 14:00:00.000000',50.00,0,'Finalizat','Suceava',40.00,0,2.00);
/*!40000 ALTER TABLE `zbor` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-01-14 17:03:34
