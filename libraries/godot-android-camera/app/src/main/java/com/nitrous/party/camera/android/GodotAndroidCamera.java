package com.nitrous.party.camera.android;

import android.widget.Toast;

import org.godotengine.godot.BuildConfig;
import org.godotengine.godot.Godot;
import org.godotengine.godot.plugin.GodotPlugin;
import org.godotengine.godot.plugin.UsedByGodot;
import org.jetbrains.annotations.NotNull;

public class GodotAndroidCamera extends GodotPlugin {

    public GodotAndroidCamera(Godot godot) {
        super(godot);
    }

    @Override @NotNull
    public String getPluginName() {
        return "godot-android-camera";
    }

    @UsedByGodot
    public void showHelloWorld() {
        runOnUiThread(() -> {
            Toast.makeText(getActivity(), "Hello World", Toast.LENGTH_LONG).show();
        });
    }
}
