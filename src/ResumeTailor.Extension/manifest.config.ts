import { defineManifest } from "@crxjs/vite-plugin";

export default defineManifest({
  manifest_version: 3,
  name: "ResumeTailor",
  description:
    "Analyzes jobs listings and selected the most relevant user-authored resume bullets.",
  version: "0.1.0",

  action: {
    default_popup: "index.html",
    default_title: "ResumeTailor",
  },
});
