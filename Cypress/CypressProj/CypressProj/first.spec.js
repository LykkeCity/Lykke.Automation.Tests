/// <reference types="Cypress" />

context('Lykke first test', () => {
    beforeEach(() => {
        cy.visit('https://lykke.com')
    })


    it('sample', () => {
        // https://on.cypress.io/type
        expect(3).to.equals(3)

        cy.get('.header_login__title').click()
    })

    it('sample', () => {
        // https://on.cypress.io/type
        expect(3).to.equals(3)

        cy.get('.header_login__title').click()
    })
})