-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Cze 17, 2025 at 04:58 PM
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
  `email` varchar(255) NOT NULL,
  `first_name` varchar(255) NOT NULL,
  `last_name` varchar(255) NOT NULL,
  `phone_number` varchar(255) NOT NULL,
  `work_since` date NOT NULL,
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
(8, 'Rekrutacja', '2025-06-18 10:00:00.000000', NULL, '2025-06-18 09:00:00.000000');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `meeting_participants`
--

CREATE TABLE `meeting_participants` (
  `id` bigint(20) NOT NULL,
  `meeting_id` bigint(20) NOT NULL,
  `employee_id` bigint(20) DEFAULT NULL,
  `candidate_id` bigint(20) DEFAULT NULL,
  `client_id` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `meeting_participants`
--

INSERT INTO `meeting_participants` (`id`, `meeting_id`, `employee_id`, `candidate_id`, `client_id`) VALUES
(6, 8, 7, NULL, NULL),
(7, 8, NULL, 5, NULL),
(8, 8, NULL, NULL, 3);

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
  `order_id` bigint(20) DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `notification`
--

INSERT INTO `notification` (`id`, `msg_from`, `msg_to`, `title`, `tag`, `message`, `is_read`, `project_card_id`, `project_id`, `order_id`, `created_at`) VALUES
(120, 11, 10, 'Nowe zlecenie do podpisu', 'newOrder', 'Handlowiec Piotr P przesłał Państwu zlecenie do podpisu. Proszę zapoznać się z treścią i podpisać zlecenie.', 0, NULL, NULL, 15, '2025-06-17 16:24:50'),
(121, 10, 3, 'Zlecenie podpisane', 'orderSigned', 'Klient c podpisał zlecenie. Wyślij kartę projektu do wypełnienia.', 0, NULL, NULL, 15, '2025-06-17 16:25:08'),
(122, 10, 6, 'Zlecenie podpisane', 'orderSigned', 'Klient c podpisał zlecenie. Wyślij kartę projektu do wypełnienia.', 0, NULL, NULL, 15, '2025-06-17 16:25:08'),
(123, 6, 10, 'Karta Projektu', 'empty', 'Dzień dobry,\r\n\r\n                Jestem Tomasz Niedziółka z firmy Syzyf. Zwracam się do Państwa z prośbą o wypełnienie karty projektu, która pozwoli nam lepiej zrozumieć Państwa potrzeby i oczekiwania.\r\n\r\n                Karta projektu zawiera kluczowe informacje niezbędne do prawidłowego zaplanowania i realizacji współpracy. Prosimy o szczegółowe wypełnienie wszystkich sekcji, co umożliwi nam przygotowanie oferty maksymalnie dopasowanej do Państwa wymagań.\r\n\r\n                W przypadku pytań lub wątpliwości dotyczących wypełniania karty, proszę o kontakt pod tym adresem. Jesteśmy do Państwa dyspozycji i chętnie udzielimy wszelkich wyjaśnień.\r\n\r\n                Dziękujemy za zainteresowanie naszymi usługami i wyrażamy nadzieję na owocną współpracę.\r\n\r\n                Z poważaniem,\r\n                Tomasz Niedziółka\r\n                Firma Syzyf\r\n\r\n                ---\r\n                Karta projektu zostanie dostarczona w osobnym komunikacie.', 0, NULL, NULL, NULL, '2025-06-17 16:25:25'),
(124, 10, 6, 'Nowa karta projektu', 'fulfilled', 'Klient Google utworzył nową kartę projektu \'hsdgfvd\'.', 0, 24, NULL, NULL, '0001-01-01 00:00:00'),
(125, 6, 10, 'Karta Projektu', 'firstAccepted', 'Dzień dobry,\n\nKarta projektu hsdgfvd została wstępnie zaakceptowana przez pracowników (wsparcia). \nProszę czekać na dalsze powiadomienia związane z jej zatwierdzeniem.', 0, NULL, NULL, NULL, '2025-06-17 16:26:43'),
(126, 6, 12, 'Przydzielenie karty projektu', 'projectAssignmentRequest', 'Dzień dobry, jestem Tomasz Niedziółka. Chciałbym Ci przydzielić tę kartę projektu: hsdgfvd. Proszę o podgląd, przyjęcie lub odrzucenie.', 0, 24, NULL, NULL, '0001-01-01 00:00:00'),
(127, 12, 10, 'Przydzielenie karty projektu', 'projectAssignmentReplay', 'Dzień dobry, jestem Seba P. Będę prowadził tę kartę projektu: hsdgfvd.', 0, 24, NULL, NULL, '2025-06-17 16:27:08'),
(128, 12, 11, 'Przydzielenie karty projektu', 'projectAssignmentReplay', 'Dzień dobry, jestem Seba P. Będę prowadził tę kartę projektu: hsdgfvd.', 0, 24, NULL, NULL, '2025-06-17 16:27:08'),
(129, 11, 10, 'Nowe zlecenie do podpisu', 'newOrder', 'Handlowiec Piotr P przesłał Państwu zlecenie do podpisu. Proszę zapoznać się z treścią i podpisać zlecenie.', 0, NULL, NULL, 16, '2025-06-17 16:34:48'),
(130, 10, 3, 'Zlecenie podpisane', 'orderSigned', 'Klient c podpisał zlecenie. Wyślij kartę projektu do wypełnienia.', 0, NULL, NULL, 16, '2025-06-17 16:35:16'),
(131, 10, 6, 'Zlecenie podpisane', 'orderSigned', 'Klient c podpisał zlecenie. Wyślij kartę projektu do wypełnienia.', 0, NULL, NULL, 16, '2025-06-17 16:35:16'),
(132, 6, 10, 'Karta Projektu', 'empty', 'Dzień dobry,\r\n\r\n                Jestem Tomasz Niedziółka z firmy Syzyf. Zwracam się do Państwa z prośbą o wypełnienie karty projektu, która pozwoli nam lepiej zrozumieć Państwa potrzeby i oczekiwania.\r\n\r\n                Karta projektu zawiera kluczowe informacje niezbędne do prawidłowego zaplanowania i realizacji współpracy. Prosimy o szczegółowe wypełnienie wszystkich sekcji, co umożliwi nam przygotowanie oferty maksymalnie dopasowanej do Państwa wymagań.\r\n\r\n                W przypadku pytań lub wątpliwości dotyczących wypełniania karty, proszę o kontakt pod tym adresem. Jesteśmy do Państwa dyspozycji i chętnie udzielimy wszelkich wyjaśnień.\r\n\r\n                Dziękujemy za zainteresowanie naszymi usługami i wyrażamy nadzieję na owocną współpracę.\r\n\r\n                Z poważaniem,\r\n                Tomasz Niedziółka\r\n                Firma Syzyf\r\n\r\n                ---\r\n                Karta projektu zostanie dostarczona w osobnym komunikacie.', 0, NULL, NULL, NULL, '2025-06-17 16:35:34'),
(133, 10, 6, 'Nowa karta projektu', 'fulfilled', 'Klient Google utworzył nową kartę projektu \'gfhnfd\'.', 0, 25, NULL, NULL, '0001-01-01 00:00:00'),
(134, 6, 10, 'Karta Projektu', 'firstAccepted', 'Dzień dobry,\n\nKarta projektu gfhnfd została wstępnie zaakceptowana przez pracowników (wsparcia). \nProszę czekać na dalsze powiadomienia związane z jej zatwierdzeniem.', 0, NULL, NULL, NULL, '2025-06-17 16:36:41'),
(135, 6, 12, 'Przydzielenie karty projektu', 'projectAssignmentRequest', 'Dzień dobry, jestem Tomasz Niedziółka. Chciałbym Ci przydzielić tę kartę projektu: gfhnfd. Proszę o podgląd, przyjęcie lub odrzucenie.', 0, 25, NULL, NULL, '0001-01-01 00:00:00'),
(136, 12, 10, 'Przydzielenie karty projektu', 'projectAssignmentReplay', 'Dzień dobry, jestem Seba P. Będę prowadził tę kartę projektu: gfhnfd.', 0, 25, NULL, NULL, '2025-06-17 16:37:06'),
(137, 12, 11, 'Przydzielenie karty projektu', 'projectAssignmentReplay', 'Dzień dobry, jestem Seba P. Będę prowadził tę kartę projektu: gfhnfd.', 0, 25, NULL, NULL, '2025-06-17 16:37:06');

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
(15, 3, 6, 'ZLECENIE NA USŁUGI REKRUTACYJNE\r\n\r\nSzanowny Kliencie,\r\n\r\nNiniejszym przedstawiamy Państwu zlecenie na świadczenie usług rekrutacyjnych zgodnie z poniższymi warunkami:\r\n\r\n1. PRZEDMIOT ZLECENIA\r\n   - Przeprowadzenie procesu rekrutacyjnego zgodnie z wymaganiami określonymi w karcie projektu\r\n   - Preselekcja kandydatów na podstawie profilu stanowiska\r\n   - Przeprowadzenie wywiadów wstępnych\r\n   - Przedstawienie najlepszych kandydatów do ostatecznej selekcji\r\n\r\n2. ZAKRES USŁUG\r\n   - Analiza potrzeb rekrutacyjnych\r\n   - Opracowanie profilu kandydata\r\n   - Publikacja ogłoszenia w odpowiednich kanałach\r\n   - Wstępna selekcja aplikacji\r\n   - Przeprowadzenie wywiadów rekrutacyjnych\r\n   - Weryfikacja referencji\r\n   - Przedstawienie rekomendacji\r\n\r\n3. WARUNKI WSPÓŁPRACY\r\n   - Czas realizacji: zgodnie z harmonogramem projektu\r\n   - Forma płatności: według umowy ramowej\r\n   - Gwarancja: 3 miesiące od daty zatrudnienia\r\n\r\n4. DOKUMENTY WYMAGANE\r\n   - Karta projektu z szczegółowym opisem stanowiska\r\n   - Profil kandydata\r\n   - Dodatkowe wymagania i preferencje\r\n\r\nPodpisując niniejsze zlecenie, Klient wyraża zgodę na rozpoczęcie procesu rekrutacyjnego oraz zobowiązuje się do dostarczenia niezbędnych informacji w postaci karty projektu.\r\n\r\nData wystawienia: 17.06.2025\r\n\r\nZlecenie wymaga podpisu elektronicznego Klienta dla aktywacji procesu rekrutacyjnego.', 1, '2025-06-17 14:25:08', '2025-06-17 14:24:50', 1, NULL),
(16, 3, 6, 'ZLECENIE NA USŁUGI REKRUTACYJNE\r\n\r\nSzanowny Kliencie,\r\n\r\nNiniejszym przedstawiamy Państwu zlecenie na świadczenie usług rekrutacyjnych zgodnie z poniższymi warunkami:\r\n\r\n1. PRZEDMIOT ZLECENIA\r\n   - Przeprowadzenie procesu rekrutacyjnego zgodnie z wymaganiami określonymi w karcie projektu\r\n   - Preselekcja kandydatów na podstawie profilu stanowiska\r\n   - Przeprowadzenie wywiadów wstępnych\r\n   - Przedstawienie najlepszych kandydatów do ostatecznej selekcji\r\n\r\n2. ZAKRES USŁUG\r\n   - Analiza potrzeb rekrutacyjnych\r\n   - Opracowanie profilu kandydata\r\n   - Publikacja ogłoszenia w odpowiednich kanałach\r\n   - Wstępna selekcja aplikacji\r\n   - Przeprowadzenie wywiadów rekrutacyjnych\r\n   - Weryfikacja referencji\r\n   - Przedstawienie rekomendacji\r\n\r\n3. WARUNKI WSPÓŁPRACY\r\n   - Czas realizacji: zgodnie z harmonogramem projektu\r\n   - Forma płatności: według umowy ramowej\r\n   - Gwarancja: 3 miesiące od daty zatrudnienia\r\n\r\n4. DOKUMENTY WYMAGANE\r\n   - Karta projektu z szczegółowym opisem stanowiska\r\n   - Profil kandydata\r\n   - Dodatkowe wymagania i preferencje\r\n\r\nPodpisując niniejsze zlecenie, Klient wyraża zgodę na rozpoczęcie procesu rekrutacyjnego oraz zobowiązuje się do dostarczenia niezbędnych informacji w postaci karty projektu.\r\n\r\nData wystawienia: 17.06.2025\r\n\r\nZlecenie wymaga podpisu elektronicznego Klienta dla aktywacji procesu rekrutacyjnego.', 1, '2025-06-17 14:35:16', '2025-06-17 14:34:48', 1, NULL);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `positions`
--

CREATE TABLE `positions` (
  `id` bigint(20) NOT NULL,
  `position_name` varchar(255) NOT NULL
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
  `job_title` varchar(255) NOT NULL,
  `department` varchar(255) NOT NULL,
  `main_duties` text NOT NULL,
  `additional_duties` text DEFAULT NULL,
  `development_opportunities` text DEFAULT NULL,
  `planned_hiring_date` date NOT NULL,
  `preferred_study_fields` varchar(255) NOT NULL,
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
  `working_hours` varchar(255) NOT NULL,
  `other_remarks` text DEFAULT NULL,
  `status` enum('InProgress','Completed') NOT NULL,
  `job_levels` varchar(255) NOT NULL,
  `education` varchar(255) NOT NULL,
  `employment_forms` varchar(255) NOT NULL,
  `work_modes` varchar(255) NOT NULL,
  `project_card_id` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `projects`
--

INSERT INTO `projects` (`id`, `client_id`, `recruiter_id`, `number_of_people`, `is_salary_visible`, `job_title`, `department`, `main_duties`, `additional_duties`, `development_opportunities`, `planned_hiring_date`, `preferred_study_fields`, `additional_certifications`, `required_experience`, `preferred_experience`, `required_skills`, `preferred_skills`, `required_languages`, `preferred_languages`, `gross_salary`, `bonus_system`, `additional_benefits`, `work_tools`, `work_place`, `working_hours`, `other_remarks`, `status`, `job_levels`, `education`, `employment_forms`, `work_modes`, `project_card_id`) VALUES
(11, 3, 7, 12, 1, 'hsdgfvd', 'fdghnfdgbsfd', 'dfsghbdsfhbfgds', '', NULL, '2025-06-18', '', '', 'gfdhfgjnfdghbn', '', 'sghfgsbgfdsbvsd', '', 'dfghbsfdbvsfdgbds', '', '5000', 1, '', '', 'sdfhiklosdfhbgvfda', '8-18', '', 'Completed', 'Specjalista/ka', 'Licencjat/Inżynier', 'Umowa zlecenie, Umowa o dzieło', 'Hybrydowa', 24),
(12, 3, 7, 12, 1, 'gfhnfd', 'sdfhbdsfgd', 'sdfghbsdf', '', NULL, '2025-06-18', '', '', 'shgfgjnfgds', '', 'jgfgshbgsfhg', '', 'fghdjnmghjsfg', '', 'hsfghsfghs', 1, '', '', 'fgjfdghfgdjfgd', '8-18', '', 'InProgress', 'Starszy Specjalista/ka', 'Licencjat/Inżynier', 'Umowa zlecenie', 'Hybrydowa', 25);

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
(12, 24, 1, '2025-06-17 16:27:14', 6, 1, '2025-06-17 16:26:43', 1, '2025-06-17 16:27:28'),
(13, 25, 1, '2025-06-17 16:37:38', 6, 1, '2025-06-17 16:36:41', 1, '2025-06-17 16:37:25');

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
  `status` enum('Processed','Pending','Accepted') NOT NULL DEFAULT 'Pending',
  `job_levels` varchar(255) NOT NULL,
  `education` varchar(255) NOT NULL,
  `employment_forms` varchar(255) NOT NULL,
  `work_modes` varchar(255) NOT NULL,
  `is_accepted` tinyint(1) NOT NULL DEFAULT 0,
  `recruiter_id` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `project_cards`
--

INSERT INTO `project_cards` (`id`, `client_id`, `number_of_people`, `is_salary_visible`, `job_title`, `department`, `main_duties`, `additional_duties`, `development_opportunities`, `planned_hiring_date`, `preferred_study_fields`, `additional_certifications`, `required_experience`, `preferred_experience`, `required_skills`, `preferred_skills`, `required_languages`, `preferred_languages`, `gross_salary`, `bonus_system`, `additional_benefits`, `work_tools`, `work_place`, `working_hours`, `other_remarks`, `status`, `job_levels`, `education`, `employment_forms`, `work_modes`, `is_accepted`, `recruiter_id`) VALUES
(24, 3, 12, 1, 'hsdgfvd', 'fdghnfdgbsfd', 'dfsghbdsfhbfgds', '', NULL, '2025-06-18', '', '', 'gfdhfgjnfdghbn', '', 'sghfgsbgfdsbvsd', '', 'dfghbsfdbvsfdgbds', '', '5000', 1, '', '', 'sdfhiklosdfhbgvfda', '8-18', '', 'Accepted', 'Specjalista/ka', 'Licencjat/Inżynier', 'Umowa zlecenie, Umowa o dzieło', 'Hybrydowa', 1, 7),
(25, 3, 12, 1, 'gfhnfd', 'sdfhbdsfgd', 'sdfghbsdf', '', NULL, '2025-06-18', '', '', 'shgfgjnfgds', '', 'jgfgshbgsfhg', '', 'fghdjnmghjsfg', '', 'hsfghsfghs', 1, '', '', 'fgjfdghfgdjfgd', '8-18', '', 'Accepted', 'Starszy Specjalista/ka', 'Licencjat/Inżynier', 'Umowa zlecenie', 'Hybrydowa', 1, 7);

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
  ADD PRIMARY KEY (`id`),
  ADD KEY `MeetingId` (`meeting_id`),
  ADD KEY `EmployeeId` (`employee_id`),
  ADD KEY `CandidateId` (`candidate_id`),
  ADD KEY `ClientId` (`client_id`);

--
-- Indeksy dla tabeli `notification`
--
ALTER TABLE `notification`
  ADD PRIMARY KEY (`id`),
  ADD KEY `project_card_id` (`project_card_id`),
  ADD KEY `msg_from` (`msg_from`),
  ADD KEY `msg_to` (`msg_to`),
  ADD KEY `order_id` (`order_id`),
  ADD KEY `project_id` (`project_id`);

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
  ADD KEY `idx_client_id` (`client_id`),
  ADD KEY `recruiter_id` (`recruiter_id`),
  ADD KEY `project_card_id` (`project_card_id`);

--
-- Indeksy dla tabeli `project_acceptance`
--
ALTER TABLE `project_acceptance`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_project_acceptances_project_card` (`project_card_id`),
  ADD KEY `support_id` (`support_id`);

--
-- Indeksy dla tabeli `project_cards`
--
ALTER TABLE `project_cards`
  ADD PRIMARY KEY (`id`),
  ADD KEY `client_id` (`client_id`),
  ADD KEY `recruiter_id` (`recruiter_id`);

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
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `meeting_participants`
--
ALTER TABLE `meeting_participants`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `notification`
--
ALTER TABLE `notification`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=138;

--
-- AUTO_INCREMENT for table `orders`
--
ALTER TABLE `orders`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `positions`
--
ALTER TABLE `positions`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `projects`
--
ALTER TABLE `projects`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `project_acceptance`
--
ALTER TABLE `project_acceptance`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `project_cards`
--
ALTER TABLE `project_cards`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

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
  ADD CONSTRAINT `meeting_participants_ibfk_1` FOREIGN KEY (`meeting_id`) REFERENCES `meetings` (`id`),
  ADD CONSTRAINT `meeting_participants_ibfk_2` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`),
  ADD CONSTRAINT `meeting_participants_ibfk_3` FOREIGN KEY (`candidate_id`) REFERENCES `candidates` (`id`),
  ADD CONSTRAINT `meeting_participants_ibfk_4` FOREIGN KEY (`client_id`) REFERENCES `clients` (`id`);

--
-- Constraints for table `notification`
--
ALTER TABLE `notification`
  ADD CONSTRAINT `notification_ibfk_1` FOREIGN KEY (`project_card_id`) REFERENCES `project_cards` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `notification_ibfk_2` FOREIGN KEY (`msg_from`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `notification_ibfk_3` FOREIGN KEY (`msg_to`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `notification_ibfk_4` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `notification_ibfk_5` FOREIGN KEY (`project_id`) REFERENCES `projects` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`client_id`) REFERENCES `clients` (`id`),
  ADD CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`sales_id`) REFERENCES `employees` (`id`),
  ADD CONSTRAINT `orders_ibfk_3` FOREIGN KEY (`project_card_id`) REFERENCES `project_cards` (`id`);

--
-- Constraints for table `projects`
--
ALTER TABLE `projects`
  ADD CONSTRAINT `projects_ibfk_1` FOREIGN KEY (`client_id`) REFERENCES `clients` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `projects_ibfk_2` FOREIGN KEY (`recruiter_id`) REFERENCES `employees` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `projects_ibfk_3` FOREIGN KEY (`project_card_id`) REFERENCES `project_cards` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `project_acceptance`
--
ALTER TABLE `project_acceptance`
  ADD CONSTRAINT `FK_project_acceptances_project_card` FOREIGN KEY (`project_card_id`) REFERENCES `project_cards` (`id`),
  ADD CONSTRAINT `project_acceptance_ibfk_1` FOREIGN KEY (`support_id`) REFERENCES `employees` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `project_cards`
--
ALTER TABLE `project_cards`
  ADD CONSTRAINT `project_cards_ibfk_1` FOREIGN KEY (`recruiter_id`) REFERENCES `employees` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `project_cards_ibfk_2` FOREIGN KEY (`client_id`) REFERENCES `clients` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`candidate_id`) REFERENCES `candidates` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `users_ibfk_2` FOREIGN KEY (`client_id`) REFERENCES `clients` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `users_ibfk_3` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
