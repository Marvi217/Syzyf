-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Cze 03, 2025 at 01:13 PM
-- Wersja serwera: 10.4.32-MariaDB
-- Wersja PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `syzyf`
--

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `candidates`
--

CREATE TABLE `candidates` (
  `id` bigint(20) NOT NULL,
  `first_name` varchar(255) NOT NULL,
  `last_name` varchar(255) NOT NULL,
  `address` varchar(255) DEFAULT NULL,
  `cv` longblob DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `phone_number` varchar(255) DEFAULT NULL,
  `positions` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `candidateselection`
--

CREATE TABLE `candidateselection` (
  `id` bigint(20) NOT NULL,
  `selection_date` date DEFAULT NULL,
  `status` enum('Accepted','Pending','Rejected') DEFAULT NULL,
  `candidate_id` bigint(20) NOT NULL,
  `project_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `clients`
--

CREATE TABLE `clients` (
  `id` bigint(20) NOT NULL,
  `nip` varchar(255) DEFAULT NULL,
  `address` varchar(255) DEFAULT NULL,
  `company` varchar(255) DEFAULT NULL,
  `contact_email` varchar(255) DEFAULT NULL,
  `contact_number` varchar(255) DEFAULT NULL,
  `contact_person_email` varchar(255) DEFAULT NULL,
  `contact_person_number` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `employees`
--

CREATE TABLE `employees` (
  `id` bigint(20) NOT NULL,
  `email` varchar(255) DEFAULT NULL,
  `first_name` varchar(255) DEFAULT NULL,
  `last_name` varchar(255) DEFAULT NULL,
  `phone_number` varchar(255) DEFAULT NULL,
  `work_since` date DEFAULT NULL,
  `position` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `employees`
--

INSERT INTO `employees` (`id`, `email`, `first_name`, `last_name`, `phone_number`, `work_since`, `position`) VALUES
(1, 'admin@gmailcom', 'Admin', 'admin', '100100100', '2025-05-26', 1),
(3, 't@gmail.com', 'Tomasz', 'Niedziolka', '123456789', '2025-05-26', 3),
(4, 't.n@gmail.com', 'Tomasz', 'Niedziółka', '234123678', '2025-06-02', 1);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `evaluation`
--

CREATE TABLE `evaluation` (
  `id` bigint(20) NOT NULL,
  `comment` text DEFAULT NULL,
  `evaluation_date` date DEFAULT NULL,
  `score` int(11) DEFAULT NULL,
  `candidate_id` bigint(20) NOT NULL,
  `client_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `invoices`
--

CREATE TABLE `invoices` (
  `id` int(11) NOT NULL,
  `amount` varchar(50) NOT NULL,
  `company` varchar(50) NOT NULL,
  `invoice_number` varchar(50) NOT NULL,
  `price` varchar(50) NOT NULL,
  `range_value` varchar(50) NOT NULL,
  `service_range` varchar(50) NOT NULL,
  `client_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `meetings`
--

CREATE TABLE `meetings` (
  `id` bigint(20) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  `end_time` datetime(6) DEFAULT NULL,
  `participants` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`participants`)),
  `start_time` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `meeting_participants`
--

CREATE TABLE `meeting_participants` (
  `Id` bigint(20) NOT NULL,
  `MeetingId` bigint(20) NOT NULL,
  `EmployeeId` bigint(20) DEFAULT NULL,
  `CandidateId` bigint(20) DEFAULT NULL,
  `ClientId` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `notification`
--

CREATE TABLE `notification` (
  `id` bigint(20) NOT NULL,
  `msg_from` bigint(20) NOT NULL,
  `msg_to` text NOT NULL,
  `title` varchar(255) NOT NULL,
  `message` varchar(255) DEFAULT NULL,
  `is_read` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `notification`
--

INSERT INTO `notification` (`id`, `msg_from`, `msg_to`, `title`, `message`, `is_read`) VALUES
(1, 1, '[4]', 'Test', 'Tetsowane jest', 0);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `positions`
--

CREATE TABLE `positions` (
  `id` bigint(20) NOT NULL,
  `position_name` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `positions`
--

INSERT INTO `positions` (`id`, `position_name`) VALUES
(1, 'Admin'),
(2, 'Handlarz'),
(3, 'Wsparcie'),
(4, 'Rekruter');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `projects`
--

CREATE TABLE `projects` (
  `id` bigint(20) NOT NULL,
  `details` text DEFAULT NULL,
  `name` varchar(255) DEFAULT NULL,
  `start` date DEFAULT NULL,
  `status` enum('Completed','InProgress','Planned') DEFAULT NULL,
  `client_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `projects-employees`
--

CREATE TABLE `projects-employees` (
  `id` bigint(20) NOT NULL,
  `employee_id` bigint(20) DEFAULT NULL,
  `project_id` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `projectversions`
--

CREATE TABLE `projectversions` (
  `id` bigint(20) NOT NULL,
  `changed_time` datetime(6) DEFAULT NULL,
  `details` text DEFAULT NULL,
  `employee_id` bigint(20) DEFAULT NULL,
  `project_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `recruitmentmeetings`
--

CREATE TABLE `recruitmentmeetings` (
  `id` bigint(20) NOT NULL,
  `title` varchar(255) DEFAULT NULL,
  `location` varchar(255) DEFAULT NULL,
  `meeting_date` date DEFAULT NULL,
  `meeting_type` varchar(255) DEFAULT NULL,
  `candidate_id` bigint(20) NOT NULL,
  `client_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `users`
--

CREATE TABLE `users` (
  `id` bigint(20) NOT NULL,
  `email` varchar(255) DEFAULT NULL,
  `login` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `employee_id` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `email`, `login`, `password`, `employee_id`) VALUES
(1, NULL, '1', '$2a$10$U7ght0DyR3BCaT0rqmEWNeKo7aGEH/NTQAGGDcuP/vyZNPco.E6pO', 1),
(3, NULL, '2', '$2a$10$l3f81dU.n156B2AKxX8pLui2mY1sHQ/0e6nTKK0n2nXHV7Q8eKfQW', 3),
(4, NULL, 'testuser', 'haslo123', NULL),
(5, NULL, 'Admin', '1', 4);

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `candidates`
--
ALTER TABLE `candidates`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `candidateselection`
--
ALTER TABLE `candidateselection`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FKrlgx5rhxm6s9wqke0vu84dgpy` (`candidate_id`),
  ADD KEY `FK74p3ugtaapuefwuo8qyjllg8q` (`project_id`);

--
-- Indeksy dla tabeli `clients`
--
ALTER TABLE `clients`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `employees`
--
ALTER TABLE `employees`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK37pcuift0oxh29rxdeertk11n` (`position`);

--
-- Indeksy dla tabeli `evaluation`
--
ALTER TABLE `evaluation`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FKtlqle7jgjhu3p5oq1h8a7x577` (`candidate_id`),
  ADD KEY `FK4tv23wmonelk56hav5ey8f59a` (`client_id`);

--
-- Indeksy dla tabeli `invoices`
--
ALTER TABLE `invoices`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK9ioqm804urbgy986pdtwqtl0x` (`client_id`);

--
-- Indeksy dla tabeli `meetings`
--
ALTER TABLE `meetings`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `meeting_participants`
--
ALTER TABLE `meeting_participants`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `MeetingId` (`MeetingId`),
  ADD KEY `EmployeeId` (`EmployeeId`),
  ADD KEY `CandidateId` (`CandidateId`),
  ADD KEY `ClientId` (`ClientId`);

--
-- Indeksy dla tabeli `notification`
--
ALTER TABLE `notification`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `positions`
--
ALTER TABLE `positions`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `projects`
--
ALTER TABLE `projects`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FKksdiyuily2f4ca2y53k07pmq` (`client_id`);

--
-- Indeksy dla tabeli `projects-employees`
--
ALTER TABLE `projects-employees`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FKdid31jlghyg4721cyyulaue8h` (`employee_id`),
  ADD KEY `FKiwf9jobo18083tqqyb7g6xe2l` (`project_id`);

--
-- Indeksy dla tabeli `projectversions`
--
ALTER TABLE `projectversions`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FKkrw0lri0bpmqit2px7eul163l` (`employee_id`),
  ADD KEY `FKecgv0u8rxetowkes6f3d6e538` (`project_id`);

--
-- Indeksy dla tabeli `recruitmentmeetings`
--
ALTER TABLE `recruitmentmeetings`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FKqocug7k7x65ifk7852af4mbm0` (`candidate_id`),
  ADD KEY `FKtoxclyjc589lc4peirv8i24fp` (`client_id`);

--
-- Indeksy dla tabeli `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `UKd1s31g1a7ilra77m65xmka3ei` (`employee_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `candidates`
--
ALTER TABLE `candidates`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `candidateselection`
--
ALTER TABLE `candidateselection`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `clients`
--
ALTER TABLE `clients`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `employees`
--
ALTER TABLE `employees`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `evaluation`
--
ALTER TABLE `evaluation`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `invoices`
--
ALTER TABLE `invoices`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `meetings`
--
ALTER TABLE `meetings`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `meeting_participants`
--
ALTER TABLE `meeting_participants`
  MODIFY `Id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `notification`
--
ALTER TABLE `notification`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `positions`
--
ALTER TABLE `positions`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `projects`
--
ALTER TABLE `projects`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `projects-employees`
--
ALTER TABLE `projects-employees`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `projectversions`
--
ALTER TABLE `projectversions`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `recruitmentmeetings`
--
ALTER TABLE `recruitmentmeetings`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `candidateselection`
--
ALTER TABLE `candidateselection`
  ADD CONSTRAINT `FK74p3ugtaapuefwuo8qyjllg8q` FOREIGN KEY (`project_id`) REFERENCES `projects` (`id`),
  ADD CONSTRAINT `FKrlgx5rhxm6s9wqke0vu84dgpy` FOREIGN KEY (`candidate_id`) REFERENCES `candidates` (`id`);

--
-- Constraints for table `employees`
--
ALTER TABLE `employees`
  ADD CONSTRAINT `FK37pcuift0oxh29rxdeertk11n` FOREIGN KEY (`position`) REFERENCES `positions` (`id`);

--
-- Constraints for table `evaluation`
--
ALTER TABLE `evaluation`
  ADD CONSTRAINT `FK4tv23wmonelk56hav5ey8f59a` FOREIGN KEY (`client_id`) REFERENCES `clients` (`id`),
  ADD CONSTRAINT `FKtlqle7jgjhu3p5oq1h8a7x577` FOREIGN KEY (`candidate_id`) REFERENCES `candidates` (`id`);

--
-- Constraints for table `invoices`
--
ALTER TABLE `invoices`
  ADD CONSTRAINT `FK9ioqm804urbgy986pdtwqtl0x` FOREIGN KEY (`client_id`) REFERENCES `clients` (`id`);

--
-- Constraints for table `meeting_participants`
--
ALTER TABLE `meeting_participants`
  ADD CONSTRAINT `meeting_participants_ibfk_1` FOREIGN KEY (`MeetingId`) REFERENCES `meetings` (`id`),
  ADD CONSTRAINT `meeting_participants_ibfk_2` FOREIGN KEY (`EmployeeId`) REFERENCES `employees` (`id`),
  ADD CONSTRAINT `meeting_participants_ibfk_3` FOREIGN KEY (`CandidateId`) REFERENCES `candidates` (`id`),
  ADD CONSTRAINT `meeting_participants_ibfk_4` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`id`);

--
-- Constraints for table `projects`
--
ALTER TABLE `projects`
  ADD CONSTRAINT `FKksdiyuily2f4ca2y53k07pmq` FOREIGN KEY (`client_id`) REFERENCES `clients` (`id`);

--
-- Constraints for table `projects-employees`
--
ALTER TABLE `projects-employees`
  ADD CONSTRAINT `FKdid31jlghyg4721cyyulaue8h` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`),
  ADD CONSTRAINT `FKiwf9jobo18083tqqyb7g6xe2l` FOREIGN KEY (`project_id`) REFERENCES `projects` (`id`);

--
-- Constraints for table `projectversions`
--
ALTER TABLE `projectversions`
  ADD CONSTRAINT `FKecgv0u8rxetowkes6f3d6e538` FOREIGN KEY (`project_id`) REFERENCES `projects` (`id`),
  ADD CONSTRAINT `FKkrw0lri0bpmqit2px7eul163l` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`);

--
-- Constraints for table `recruitmentmeetings`
--
ALTER TABLE `recruitmentmeetings`
  ADD CONSTRAINT `FKqocug7k7x65ifk7852af4mbm0` FOREIGN KEY (`candidate_id`) REFERENCES `candidates` (`id`),
  ADD CONSTRAINT `FKtoxclyjc589lc4peirv8i24fp` FOREIGN KEY (`client_id`) REFERENCES `clients` (`id`);

--
-- Constraints for table `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `FK6p2ib82uai0pj9yk1iassppgq` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
