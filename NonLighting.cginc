		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _MainTex;

		void surf (Input IN, inout SurfaceOutput o) {
			half4 color = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = color.rgb;
			o.Alpha = color.a;
		}