-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Cze 16, 2025 at 03:45 PM
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

--
-- Dumping data for table `candidates`
--

INSERT INTO `candidates` (`id`, `first_name`, `last_name`, `address`, `cv`, `email`, `phone_number`, `positions`) VALUES
(5, 'Tomasz', 'Niedziółka', 'Siedlce', NULL, 'tn89280@test.pl', '123456789', 'IT');

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

--
-- Dumping data for table `clients`
--

INSERT INTO `clients` (`id`, `nip`, `address`, `company`, `contact_email`, `contact_number`, `contact_person_email`, `contact_person_number`) VALUES
(3, '632476247260', 'Warszawa', 'Google', 'google@gmail.com', '34762896492', 'tn@gmail.com', '26348762364');

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
(4, 't.niedziolka3@gmail.com', 'Tomasz', 'Niedziółka', '507295456', '2025-06-02', 1),
(5, 'tn89280@xd.pl', 'Tomasz', 'Niedziółka', '728187291', '2025-06-10', 3),
(6, 'p@syzyf.pl', 'Piotr', 'P', '3426794723', '2025-06-10', 2),
(7, 'jdkjb', 'Seba', 'P', '8329750832094', '2025-06-14', 4);

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
  `title` varchar(255) DEFAULT NULL,
  `end_time` datetime(6) DEFAULT NULL,
  `participants` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`participants`)),
  `start_time` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `meetings`
--

INSERT INTO `meetings` (`id`, `title`, `end_time`, `participants`, `start_time`) VALUES
(1, 'Test', '2025-06-04 10:00:00.000000', NULL, '2025-06-04 09:00:00.000000'),
(2, 'ndsflkhbsvloidf', '2025-06-10 10:00:00.000000', NULL, '2025-06-10 09:00:00.000000');

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
  `msg_to` bigint(20) NOT NULL,
  `title` varchar(255) NOT NULL,
  `tag` varchar(50) NOT NULL,
  `message` text DEFAULT NULL,
  `is_read` tinyint(1) NOT NULL DEFAULT 0,
  `project_card_id` bigint(20) DEFAULT NULL,
  `project_id` bigint(20) DEFAULT NULL,
  `order_id` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `notification`
--

INSERT INTO `notification` (`id`, `msg_from`, `msg_to`, `title`, `tag`, `message`, `is_read`, `project_card_id`, `project_id`, `order_id`) VALUES
(38, 11, 10, 'Nowe zlecenie do podpisu', 'newOrder', 'Handlowiec Piotr P przesłał Państwu zlecenie do podpisu. Proszę zapoznać się z treścią i podpisać zlecenie.', 0, NULL, NULL, 8),
(39, 10, 3, 'Zlecenie podpisane', 'orderSigned', 'Klient c podpisał zlecenie. Wyślij kartę projektu do wypełnienia.', 0, NULL, NULL, 8),
(40, 10, 6, 'Zlecenie podpisane', 'orderSigned', 'Klient c podpisał zlecenie. Wyślij kartę projektu do wypełnienia.', 0, NULL, NULL, 8),
(41, 6, 10, 'Karta Projektu', 'filled', 'Dzień dobry,\r\n\r\n                Jestem Tomasz Niedziółka z firmy Syzyf. Zwracam się do Państwa z prośbą o wypełnienie karty projektu, która pozwoli nam lepiej zrozumieć Państwa potrzeby i oczekiwania.\r\n\r\n                Karta projektu zawiera kluczowe informacje niezbędne do prawidłowego zaplanowania i realizacji współpracy. Prosimy o szczegółowe wypełnienie wszystkich sekcji, co umożliwi nam przygotowanie oferty maksymalnie dopasowanej do Państwa wymagań.\r\n\r\n                W przypadku pytań lub wątpliwości dotyczących wypełniania karty, proszę o kontakt pod tym adresem. Jesteśmy do Państwa dyspozycji i chętnie udzielimy wszelkich wyjaśnień.\r\n\r\n                Dziękujemy za zainteresowanie naszymi usługami i wyrażamy nadzieję na owocną współpracę.\r\n\r\n                Z poważaniem,\r\n                Tomasz Niedziółka\r\n                Firma Syzyf\r\n\r\n                ---\r\n                Karta projektu zostanie dostarczona w osobnym komunikacie.', 0, NULL, NULL, NULL),
(42, 10, 6, 'Karta projektu', 'fulfilled', 'Karta projektu \'Nazwa stanowiska *\' został zapisany przez użytkownika c w dniu 2025-06-14 09:15.', 0, 5, NULL, NULL),
(50, 6, 10, 'Karta Projektu', 'firstAccepted', 'Dzień dobry,\n\nKarta projektu Nazwa stanowiska * została wstępnie zaakceptowana przez pracowników (wsparcia). \nProszę czekać na dalsze powiadomienia związane z jej zatwierdzeniem.', 0, NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `orders`
--

CREATE TABLE `orders` (
  `id` bigint(20) NOT NULL,
  `client_id` bigint(20) NOT NULL,
  `sales_id` bigint(20) DEFAULT NULL,
  `order_content` text NOT NULL,
  `is_signed_by_client` tinyint(1) NOT NULL DEFAULT 0,
  `signed_date` timestamp NULL DEFAULT NULL,
  `created_date` timestamp NOT NULL DEFAULT current_timestamp(),
  `status` int(11) NOT NULL,
  `project_card_id` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `orders`
--

INSERT INTO `orders` (`id`, `client_id`, `sales_id`, `order_content`, `is_signed_by_client`, `signed_date`, `created_date`, `status`, `project_card_id`) VALUES
(8, 3, 6, 'ZLECENIE NA USŁUGI REKRUTACYJNE\r\n\r\nSzanowny Kliencie,\r\n\r\nNiniejszym przedstawiamy Państwu zlecenie na świadczenie usług rekrutacyjnych zgodnie z poniższymi warunkami:\r\n\r\n1. PRZEDMIOT ZLECENIA\r\n   - Przeprowadzenie procesu rekrutacyjnego zgodnie z wymaganiami określonymi w karcie projektu\r\n   - Preselekcja kandydatów na podstawie profilu stanowiska\r\n   - Przeprowadzenie wywiadów wstępnych\r\n   - Przedstawienie najlepszych kandydatów do ostatecznej selekcji\r\n\r\n2. ZAKRES USŁUG\r\n   - Analiza potrzeb rekrutacyjnych\r\n   - Opracowanie profilu kandydata\r\n   - Publikacja ogłoszenia w odpowiednich kanałach\r\n   - Wstępna selekcja aplikacji\r\n   - Przeprowadzenie wywiadów rekrutacyjnych\r\n   - Weryfikacja referencji\r\n   - Przedstawienie rekomendacji\r\n\r\n3. WARUNKI WSPÓŁPRACY\r\n   - Czas realizacji: zgodnie z harmonogramem projektu\r\n   - Forma płatności: według umowy ramowej\r\n   - Gwarancja: 3 miesiące od daty zatrudnienia\r\n\r\n4. DOKUMENTY WYMAGANE\r\n   - Karta projektu z szczegółowym opisem stanowiska\r\n   - Profil kandydata\r\n   - Dodatkowe wymagania i preferencje\r\n\r\nPodpisując niniejsze zlecenie, Klient wyraża zgodę na rozpoczęcie procesu rekrutacyjnego oraz zobowiązuje się do dostarczenia niezbędnych informacji w postaci karty projektu.\r\n\r\nData wystawienia: 13.06.2025\r\n\r\nZlecenie wymaga podpisu elektronicznego Klienta dla aktywacji procesu rekrutacyjnego.', 1, '2025-06-13 21:14:10', '2025-06-13 21:13:57', 1, NULL);

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
(2, 'Handlowiec'),
(3, 'Wsparcie'),
(4, 'Rekruter');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `projects`
--

CREATE TABLE `projects` (
  `id` bigint(20) NOT NULL,
  `client_id` bigint(20) NOT NULL,
  `recruiter_id` bigint(20) DEFAULT NULL,
  `number_of_people` int(11) NOT NULL,
  `is_salary_visible` tinyint(1) NOT NULL,
  `contact_full_name` varchar(255) NOT NULL,
  `contact_email` varchar(255) NOT NULL,
  `contact_phone` varchar(255) DEFAULT NULL,
  `job_title` varchar(255) NOT NULL,
  `department` varchar(255) NOT NULL,
  `main_duties` text NOT NULL,
  `additional_duties` text DEFAULT NULL,
  `development_opportunities` text DEFAULT NULL,
  `planned_hiring_date` date NOT NULL,
  `preferred_study_fields` varchar(255) DEFAULT NULL,
  `additional_certifications` varchar(255) DEFAULT NULL,
  `required_experience` text NOT NULL,
  `preferred_experience` text DEFAULT NULL,
  `required_skills` text NOT NULL,
  `preferred_skills` text DEFAULT NULL,
  `required_languages` text NOT NULL,
  `preferred_languages` text DEFAULT NULL,
  `gross_salary` varchar(255) NOT NULL,
  `bonus_system` tinyint(1) NOT NULL,
  `additional_benefits` text DEFAULT NULL,
  `work_tools` text DEFAULT NULL,
  `work_place` varchar(255) NOT NULL,
  `working_hours` varchar(255) DEFAULT NULL,
  `other_remarks` text DEFAULT NULL,
  `status` enum('Planned','InProgress','Completed') NOT NULL,
  `job_levels` varchar(255) NOT NULL,
  `education` varchar(255) NOT NULL,
  `employment_forms` varchar(255) NOT NULL,
  `work_modes` varchar(255) NOT NULL,
  `project_card_id` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `project_acceptance`
--

CREATE TABLE `project_acceptance` (
  `id` bigint(20) NOT NULL,
  `project_card_id` bigint(20) NOT NULL,
  `accepted_by_recruiter` tinyint(1) NOT NULL DEFAULT 0,
  `recruiter_accepted_at` datetime DEFAULT NULL,
  `support_id` bigint(20) DEFAULT NULL,
  `accepted_by_support` tinyint(1) NOT NULL DEFAULT 0,
  `support_accepted_at` datetime DEFAULT NULL,
  `accepted_by_client` tinyint(1) NOT NULL DEFAULT 0,
  `client_accepted_at` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `project_acceptance`
--

INSERT INTO `project_acceptance` (`id`, `project_card_id`, `accepted_by_recruiter`, `recruiter_accepted_at`, `support_id`, `accepted_by_support`, `support_accepted_at`, `accepted_by_client`, `client_accepted_at`) VALUES
(0, 5, 0, NULL, 6, 1, '2025-06-14 10:47:50', 0, NULL);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `project_cards`
--

CREATE TABLE `project_cards` (
  `id` bigint(20) NOT NULL,
  `client_id` bigint(20) NOT NULL,
  `number_of_people` int(11) NOT NULL,
  `is_salary_visible` tinyint(1) NOT NULL,
  `job_title` varchar(255) NOT NULL,
  `department` varchar(255) NOT NULL,
  `main_duties` text NOT NULL,
  `additional_duties` text DEFAULT NULL,
  `development_opportunities` text DEFAULT NULL,
  `planned_hiring_date` date NOT NULL,
  `preferred_study_fields` varchar(255) DEFAULT NULL,
  `additional_certifications` varchar(255) DEFAULT NULL,
  `required_experience` text NOT NULL,
  `preferred_experience` text DEFAULT NULL,
  `required_skills` text NOT NULL,
  `preferred_skills` text DEFAULT NULL,
  `required_languages` text NOT NULL,
  `preferred_languages` text DEFAULT NULL,
  `gross_salary` varchar(255) NOT NULL,
  `bonus_system` tinyint(1) NOT NULL,
  `additional_benefits` text DEFAULT NULL,
  `work_tools` text DEFAULT NULL,
  `work_place` varchar(255) NOT NULL,
  `working_hours` varchar(255) DEFAULT NULL,
  `other_remarks` text DEFAULT NULL,
  `status` enum('Planned','InProgress','Completed') NOT NULL,
  `job_levels` varchar(255) NOT NULL,
  `education` varchar(255) NOT NULL,
  `employment_forms` varchar(255) NOT NULL,
  `work_modes` varchar(255) NOT NULL,
  `first_acceptance` tinyint(1) NOT NULL DEFAULT 0,
  `second_acceptance` tinyint(1) NOT NULL DEFAULT 0,
  `recruiter_id` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `project_cards`
--

INSERT INTO `project_cards` (`id`, `client_id`, `number_of_people`, `is_salary_visible`, `job_title`, `department`, `main_duties`, `additional_duties`, `development_opportunities`, `planned_hiring_date`, `preferred_study_fields`, `additional_certifications`, `required_experience`, `preferred_experience`, `required_skills`, `preferred_skills`, `required_languages`, `preferred_languages`, `gross_salary`, `bonus_system`, `additional_benefits`, `work_tools`, `work_place`, `working_hours`, `other_remarks`, `status`, `job_levels`, `education`, `employment_forms`, `work_modes`, `first_acceptance`, `second_acceptance`, `recruiter_id`) VALUES
(5, 3, 0, 0, 'Nazwa stanowiska *', 'Dział *', 'Główne obowiązki *', 'Dodatkowe obowiązki', NULL, '2025-06-14', 'Preferowane kierunki studiów', 'Dodatkowe uprawnienia', 'Wymagane doświadczenie *', 'Mile widziane doświadczenie', 'Wymagane umiejętności *', 'Mile widziane umiejętności', 'Wymagane języki *', 'Mile widziane języki', 'Wynagrodzenie brutto *', 0, 'Dodatkowe benefity', 'Narzędzia pracy', 'Miejsce pracy *', 'Godziny pracy *', 'Pozostałe informacje', 'Planned', '', '', '', '', 0, 0, 7);

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
  `employee_id` bigint(20) DEFAULT NULL,
  `client_id` bigint(20) DEFAULT NULL,
  `candidate_id` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `email`, `login`, `password`, `employee_id`, `client_id`, `candidate_id`) VALUES
(1, NULL, '1', '$2a$10$U7ght0DyR3BCaT0rqmEWNeKo7aGEH/NTQAGGDcuP/vyZNPco.E6pO', 1, NULL, NULL),
(3, NULL, '2', '$2a$10$l3f81dU.n156B2AKxX8pLui2mY1sHQ/0e6nTKK0n2nXHV7Q8eKfQW', 3, NULL, NULL),
(4, NULL, 'testuser', 'haslo123', NULL, NULL, NULL),
(5, NULL, 'Admin', '1', 4, NULL, NULL),
(6, NULL, '89280', '1234', 5, NULL, NULL),
(9, NULL, '1@1.pl', '1', NULL, NULL, 5),
(10, NULL, 'c', '1', NULL, 3, NULL),
(11, NULL, 'p@syzyf.pl', '1', 6, NULL, NULL),
(12, NULL, 'r', '1', 7, NULL, NULL);

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
  ADD PRIMARY KEY (`id`),
  ADD KEY `project_card_id` (`project_card_id`),
  ADD KEY `msg_from` (`msg_from`),
  ADD KEY `msg_to` (`msg_to`),
  ADD KEY `order_id` (`order_id`);

--
-- Indeksy dla tabeli `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`id`),
  ADD KEY `client_id` (`client_id`),
  ADD KEY `sales_id` (`sales_id`),
  ADD KEY `project_card_id` (`project_card_id`);

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
  ADD KEY `idx_client_id` (`client_id`);

--
-- Indeksy dla tabeli `project_acceptance`
--
ALTER TABLE `project_acceptance`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_project_acceptances_project_card` (`project_card_id`);

--
-- Indeksy dla tabeli `project_cards`
--
ALTER TABLE `project_cards`
  ADD PRIMARY KEY (`id`),
  ADD KEY `client_id` (`client_id`);

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
  ADD UNIQUE KEY `UKd1s31g1a7ilra77m65xmka3ei` (`employee_id`),
  ADD KEY `client_id` (`client_id`),
  ADD KEY `candidate_id` (`candidate_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `candidates`
--
ALTER TABLE `candidates`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `candidateselection`
--
ALTER TABLE `candidateselection`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `clients`
--
ALTER TABLE `clients`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `employees`
--
ALTER TABLE `employees`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

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
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `meeting_participants`
--
ALTER TABLE `meeting_participants`
  MODIFY `Id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `notification`
--
ALTER TABLE `notification`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=51;

--
-- AUTO_INCREMENT for table `orders`
--
ALTER TABLE `orders`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `positions`
--
ALTER TABLE `positions`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `projects`
--
ALTER TABLE `projects`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `project_cards`
--
ALTER TABLE `project_cards`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `recruitmentmeetings`
--
ALTER TABLE `recruitmentmeetings`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

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
-- Constraints for table `notification`
--
ALTER TABLE `notification`
  ADD CONSTRAINT `notification_ibfk_1` FOREIGN KEY (`project_card_id`) REFERENCES `project_cards` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `notification_ibfk_2` FOREIGN KEY (`msg_from`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `notification_ibfk_3` FOREIGN KEY (`msg_to`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `notification_ibfk_4` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`client_id`) REFERENCES `clients` (`id`),
  ADD CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`sales_id`) REFERENCES `employees` (`id`),
  ADD CONSTRAINT `orders_ibfk_3` FOREIGN KEY (`project_card_id`) REFERENCES `project_cards` (`id`);

--
-- Constraints for table `project_acceptance`
--
ALTER TABLE `project_acceptance`
  ADD CONSTRAINT `FK_project_acceptances_project_card` FOREIGN KEY (`project_card_id`) REFERENCES `project_cards` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
