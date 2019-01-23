# Sticos.Personal.Web

`npm start` to run the application

## Linting and formatting
We use tslint and prettier. Install the [Prettier VSCode extension](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode) or use the `npm run format` command.
To enable format on save for typescript files in VSCode add the following to your user settings file.
```
"[typescript]": {
    "editor.formatOnSave": true
},
```

CI will check if the files are formatted correctly. Read more about prettier [on their website](https://prettier.io/).

## Swagger
The swagger.json files are included in source control and needs to be copied to their folder if you have made changes to the backend. The files are located in `src/apis/{servicename}`
To generate the API-client code run the command `npm run generate:apis`

# Angular CLI
This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 6.0.8.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).
