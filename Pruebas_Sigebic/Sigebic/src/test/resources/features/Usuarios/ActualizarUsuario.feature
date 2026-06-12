# language: es
  # Author: Cristian

  Característica: Actualizar datos de un usuario en el sistema
    Como administrador del sistema
    Quiero poder actualizar la información de un usuario existente
    Para mantener los datos del sistema actualizados

  Antecedentes:
    Dado el administrador inicie sesion con las credenciales correctas
      | usuario              | contraseña |
      | admin@biblioteca.com | Admin1234  |
    Y se encuentre en la pagina de gestion gestion de Usuarios

  @ActualizarUsuario
  Escenario: Actualizar datos de un usuario
    Cuando el administrador modifica la informacion de un usuario existente en el sistema.
    Entonces deberia los datos del usuario actualizados