<h1>Table of Contents</h1>

- [Installation](#installation)
  - [Standard folder](#standard-folder)
  - [Custom folder](#custom-folder)
- [Usage](#usage)
  - [From main menu (extension menu)](#from-main-menu-extension-menu)
  - [From designer menu (right click on opened form, table, etc)](#from-designer-menu-right-click-on-opened-form-table-etc)
- [Addins](#addins)
  - [Create Batch](#create-batch)
    - [Step 1 : Enter name and select project](#step-1--enter-name-and-select-project)
    - [Step 2 : Select label file and enter label id and description](#step-2--select-label-file-and-enter-label-id-and-description)
    - [Step 3 : (Repeated) Enter label text for a given language](#step-3--repeated-enter-label-text-for-a-given-language)
    - [Result](#result)
  - [Create Form](#create-form)
    - [Step 1 : Enter name and select project](#step-1--enter-name-and-select-project-1)
    - [Step 2 : Select label file and enter label id and description](#step-2--select-label-file-and-enter-label-id-and-description-1)
    - [Step 3 : (Repeated) Enter label text for a given language](#step-3--repeated-enter-label-text-for-a-given-language-1)
    - [Result](#result-1)
  - [Auto Fill Pattern Design](#auto-fill-pattern-design)
    - [Step 1 : Select pattern and option](#step-1--select-pattern-and-option)
    - [Result](#result-2)
    - [Informations](#informations)

# Installation

You must download the addins dlls that you want to use along with the common [SOG_SharedUtils.dll](dlls/SOG_SharedUtils.dll)

You have two options for where to put them at, the standard addins folder or a custom one

## Standard folder

To locate the standard folder you may go through the following manipulation using the system environment variables :

![alt text](screenshots/001_install_env_vars.png)

## Custom folder

You have the possibility to put your dlls in a custom folder by adding a line to the following configuration file :

![alt text](screenshots/002_install_custom_folder.png)

# Usage

## From main menu (extension menu)

![alt text](screenshots/003_usage_mainmenu.png)

## From designer menu (right click on opened form, table, etc)

![alt text](screenshots/004_usage_designermenu.png)

# Addins

## Create Batch

This addin lets you create a new Batch Job which implies the following components :
- Contract class (with samples)
- Service class (with samples)
- Controller class (ready to be used)
- Action menu item
- Maintain privilege
- Labels & Links between components

### Step 1 : Enter name and select project

![alt text](screenshots/012_batch_step1.png)

### Step 2 : Select label file and enter label id and description

![alt text](screenshots/013_batch_step2.png)

### Step 3 : (Repeated) Enter label text for a given language

![alt text](screenshots/014_batch_step3.png)

### Result

![alt text](screenshots/015_batch_result.png)

## Create Form

This addin lets you create a new Form which implies the following components :
- Form
- Display menu item
- View & Maintain privilege
- Labels & Links between components

### Step 1 : Enter name and select project

![alt text](screenshots/005_form_step1.png)

### Step 2 : Select label file and enter label id and description

![alt text](screenshots/006_form_step2.png)

### Step 3 : (Repeated) Enter label text for a given language

![alt text](screenshots/007_form_step3.png)

### Result

![alt text](screenshots/008_form_result.png)

## Auto Fill Pattern Design

This addin lets you select a pattern for a form design and fills automatically the controls in the pattern's tree

### Step 1 : Select pattern and option

![alt text](screenshots/009_autofill_step1.png)

### Result

![alt text](screenshots/010_autofill_result.png)

### Informations

When checking the "Add optionnal controls ?" option, the addin will perform creation even for "0..n" subcontrols and keep its recursive behavior on those controls

When crossing a subpattern choice, the addin will select the only pattern if there is only one, if multiple, it will let the user decide after completion

Those points are clearly identified here :

![alt text](screenshots/011_autofill_infos.png)
