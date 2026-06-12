# language: es
  # Author: Cristian

  Característica: Registrar un nuevo usuario en el sistema
    Como administrador del sistema
    Quiero poder registrar un nuevo usuario
    Para gestionar los accesos al sistema

  Antecedentes:
    Dado el administrador inicie sesion con las credenciales correctas
      | usuario              | contraseña |
      | admin@biblioteca.com | Admin1234  |
    Y se encuentre en la pagina de gestion gestion de Usuarios

  @RegistrarUsuario
  Escenario: Crear un nuevo usuario
    Cuando el administrador crea un nuevo usuario
    Entonces deberia ver el nuevo usuario en la lista