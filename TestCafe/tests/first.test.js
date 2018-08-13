import { Selector } from 'testcafe'; // first import testcafe selectors

fixture`Getting Started`// declare the fixture
    .page`https://www.lykke.com`;  // specify the start page


//then create a test and place your code there
test('My first test', async t => {
    await t.click('button.btn_open_search')
        .typeText('#search-query', 'John Smith')
        .click('#search-button')

        // Use the assertion to check if the actual header text is equal to the expected one
        .expect(Selector('.page__title').innerText).eql('Search results');
});