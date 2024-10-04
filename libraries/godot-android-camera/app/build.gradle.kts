import com.android.build.gradle.internal.tasks.factory.dependsOn

plugins {
    alias(libs.plugins.androidLibrary)
}

var name = "godot-android-camera"
var pack = "com.nitrous.party.camera.android"

android {
    namespace = "com.nitrous.party.camera.android"
    compileSdk = 34

    buildFeatures {
        buildConfig = true
    }

    defaultConfig {
        minSdk = 21

        manifestPlaceholders["godotPluginName"] = name
        manifestPlaceholders["godotPluginPackageName"] = pack
        buildConfigField("String", "GODOT_PLUGIN_NAME", "\"${name}\"")
        setProperty("archivesBaseName", name)
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(getDefaultProguardFile("proguard-android-optimize.txt"), "proguard-rules.pro")
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_17
        targetCompatibility = JavaVersion.VERSION_17
    }
}

dependencies {
    implementation("org.godotengine:godot:4.3.0.stable")
}

// BUILD TASKS DEFINITION
val copyDebugAARToDemoAddons by tasks.registering(Copy::class) {
    description = "Copies the generated debug AAR binary to the plugin's addons directory"
    from("build/outputs/aar")
    include("$name-debug.aar")
    into("demo/addons/$name/bin/debug")
}

val copyReleaseAARToDemoAddons by tasks.registering(Copy::class) {
    description = "Copies the generated release AAR binary to the plugin's addons directory"
    from("build/outputs/aar")
    include("$name-release.aar")
    into("demo/addons/$name/bin/release")
}

val cleanDemoAddons by tasks.registering(Delete::class) {
    delete("demo/addons/$name")
}

val copyAddonsToDemo by tasks.registering(Copy::class) {
    description = "Copies the export scripts templates to the plugin's addons directory"

    dependsOn(cleanDemoAddons)
    finalizedBy(copyDebugAARToDemoAddons)
    finalizedBy(copyReleaseAARToDemoAddons)

    from("export_scripts_template")
    into("demo/addons/$name")
}

tasks.named("assemble").configure {
    finalizedBy(copyAddonsToDemo)
}

tasks.named<Delete>("clean").apply {
    dependsOn(cleanDemoAddons)
}
