plugins {
    java
    idea
    id("net.serenity-bdd.serenity-gradle-plugin") version "4.2.34"
}

group = "co.com.sigebic"
version = "1.0-SNAPSHOT"

repositories {
    mavenCentral()
}

val serenityVersion = "4.2.34"
val serenityCucumberVersion = "5.3.1"

dependencies {
    implementation("org.hamcrest:hamcrest:2.2")
    implementation("net.serenity-bdd:serenity-cucumber:$serenityCucumberVersion")
    implementation("net.serenity-bdd:serenity-core:$serenityVersion")
    implementation("net.serenity-bdd:serenity-junit:$serenityVersion")
    implementation("net.serenity-bdd:serenity-screenplay:$serenityVersion")
    implementation("net.serenity-bdd:serenity-screenplay-webdriver:$serenityVersion")
    implementation("net.serenity-bdd:serenity-screenplay-rest:$serenityVersion")
    implementation("net.serenity-bdd:serenity-ensure:2.0.49")
    implementation("org.slf4j:slf4j-simple:2.0.18")
    implementation("com.jcraft:jsch:0.1.55")
    implementation("log4j:log4j:1.2.17")
    implementation("org.apache.poi:poi:4.1.2")
    implementation("org.apache.poi:poi-ooxml:4.1.2")
    implementation("org.apache.commons:commons-math3:3.6.1")

    testImplementation("junit:junit:4.13.2")
}

java {
    sourceCompatibility = JavaVersion.VERSION_17
    targetCompatibility = JavaVersion.VERSION_17
}

tasks.withType<JavaCompile> {
    options.encoding = "UTF-8"
}


tasks.withType<Test> {
    useJUnit()
    systemProperty("cucumber.publish.quiet", "true")
    systemProperty("cucumber.junit-platform.naming-strategy", "long")
    jvmArgs(
        "--add-opens", "java.base/java.lang=ALL-UNNAMED",
        "--add-opens", "java.base/java.util=ALL-UNNAMED",
        "--add-opens", "java.base/java.lang.reflect=ALL-UNNAMED"
    )
    finalizedBy("aggregate")
}