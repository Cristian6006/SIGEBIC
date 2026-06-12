# language: es
  # Author: Cristian

  Característica: Autenticacion para la pagina Sigebic
    Como administrador del sistema
    Quiero poder realizar el inicio de sesion correctamente
    Para gestionar, procesar y acceder a datos de forma eficiente a través de internet a su información cuando sea necesario

  @Autenticacion

  Escenario: Iniciar Sesion
    Cuando el administrador inicie sesion con las credenciales correctas
      | usuario              | contraseña |
      | admin@biblioteca.com | Admin1234  |
    Entonces deberia ser redirigido a la pagina principal