﻿using Microsoft.Playwright;
using System.Text.RegularExpressions;

var tennisName = "Léo Lagrange";
var dayOfWeek = "Monday";
var hourOfDay = "13h";
var email = "email@gmail.com";
var password = "**********";
var secondPlayerLastName = "Smith";
var secondPlayerFirstName = "Will";

using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
{
    Headless = false,
});
var page = await browser.NewPageAsync();

await page.GotoAsync("https://tennis.paris.fr/tennis/jsp/site/Portal.jsp?page=tennis&view=startDefault&full=1");

var loginPage = await page.RunAndWaitForPopupAsync(async () =>
{
    await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Je me connecte" }).ClickAsync();
});

await loginPage.GetByPlaceholder("Votre adresse électronique").FillAsync(email);
await loginPage.GetByPlaceholder("Votre mot de passe").FillAsync(password);
await loginPage.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Se connecter" }).ClickAsync();

await page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "accueil" }).ClickAsync();
await page.Locator("#whereToken").GetByRole(AriaRole.Textbox).ClickAsync();
await page.Locator("#whereToken").GetByRole(AriaRole.Textbox).FillAsync(tennisName);
await page.GetByRole(AriaRole.Strong).Filter(new LocatorFilterOptions { HasTextRegex = new Regex($"^{tennisName}$") }).ClickAsync();
await page.GetByPlaceholder("Sélectionnner une date").ClickAsync();
await page.Locator("#search_form").GetByText(new Regex($"^{dayOfWeek}*.")).ClickAsync();
await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Rechercher" }).ClickAsync();
await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = hourOfDay }).ClickAsync();
await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Réserver" }).First.ClickAsync();

await page.FrameLocator("iframe[name=\"captchaImage\"]").GetByRole(AriaRole.Img).ClickAsync();
await page.GetByLabel("* Enter the heard or read characters :").FillAsync("vqMYIO");
await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Poursuivre la réservation" }).ClickAsync();

await page.GetByRole(AriaRole.Textbox).First.FillAsync(secondPlayerLastName);
await page.GetByRole(AriaRole.Textbox).Nth(1).FillAsync(secondPlayerFirstName);
await page.Locator("body").ClickAsync();
await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Etape suivante" }).ClickAsync();
await page.GetByRole(AriaRole.Cell, new PageGetByRoleOptions { NameRegex = new Regex("J’utilise . heure de mon carnet .*") }).ClickAsync();
await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Etape suivante" }).ClickAsync();
await page.GetByText("Réservation confirmée").ClickAsync();